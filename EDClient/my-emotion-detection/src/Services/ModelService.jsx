import { ServiceEmotionData } from "../Objects/EmotionData";
import { serverNotifyEmotion, serverPushEmotionData } from "./ClientService";
import { getLessonId } from "./SessionService";

const sleep = ms => new Promise(r => setTimeout(r, ms));
const intervalTime = 5000;

class ExpressionProcessor {
  constructor() {
    this.intervalId = null;
    this.expressionsData = {
      neutral: 0,
      happy: 0,
      sad: 0,
      angry: 0,
      surprised: 0,
      disgusted: 0,
      fearful: 0,
    };
    this.video = null;
    this.canvas = null; // Store the canvas element reference
  }

  async startVideo() {
    try {
      const stream = await navigator.mediaDevices.getUserMedia({ video: true });
      this.video.srcObject = stream;

      // Wait until the video element is ready to play
      await new Promise((resolve) => {
        this.video.addEventListener('loadeddata', () => {
          resolve();
        });
      });

      await this.video.play();
    } catch (error) {
      console.error(error);
    }
  }

  async processExpressions() {
    await Promise.all([
      faceapi.nets.tinyFaceDetector.loadFromUri('/models'),
      faceapi.nets.faceLandmark68Net.loadFromUri('/models'),
      faceapi.nets.faceRecognitionNet.loadFromUri('/models'),
      faceapi.nets.faceExpressionNet.loadFromUri('/models'),
    ]);

    // Create and append the video element to the DOM
    this.video = document.createElement('video');
    this.video.setAttribute('id', 'video');
    this.video.setAttribute('autoplay', 'muted');
    document.body.appendChild(this.video);
    this.video.style.display = 'none';

    // Start video stream
    await this.startVideo();

    this.canvas = faceapi.createCanvasFromMedia(this.video);
    document.body.append(this.canvas);

    let displaySize = { width: this.video.videoWidth, height: this.video.videoHeight };

    faceapi.matchDimensions(this.canvas, displaySize);

    let totalTime = 0;
    let expressionCount = 0;

    this.intervalId = setInterval(async () => {
      try {
        const detections = await faceapi
          .detectAllFaces(this.video, new faceapi.TinyFaceDetectorOptions())
          .withFaceLandmarks()
          .withFaceDescriptors()
          .withFaceExpressions();

        displaySize = { width: this.video.videoWidth, height: this.video.videoHeight };

        const resizedDetections = faceapi.resizeResults(detections, displaySize);
        this.canvas.getContext('2d').clearRect(0, 0, this.canvas.width, this.canvas.height);
        detections.forEach((detection) => {
          const expressions = detection.expressions;
          for (const expression in this.expressionsData) {
            this.expressionsData[expression] += expressions[expression];
          }
          expressionCount++;
        });

        totalTime += 100;

        if (totalTime >= intervalTime) {
          const averageExpressions = {};
          for (const expression in this.expressionsData) {
            averageExpressions[expression] =
              this.expressionsData[expression] / expressionCount;
          }
          console.log('Average Expressions:', averageExpressions);

          this.sendResultToServer(averageExpressions);

          // Clear expressions data for the next calculation
          this.expressionsData = {
            neutral: 0,
            happy: 0,
            sad: 0,
            angry: 0,
            surprised: 0,
            disgusted: 0,
            fearful: 0
          };
          totalTime = 0;
          expressionCount = 0;
        }
      } catch (error) {
        console.error(error);
      }
    }, 100);
  }

  notify() {
    serverNotifyEmotion("a");
  }

  sendResultToServer(data) {
    const emotionData = new ServiceEmotionData(
      data.neutral,
      data.happy,
      data.sad,
      data.angry,
      data.surprised,
      data.disgusted,
      data.fearful
    );
    serverPushEmotionData(getLessonId(), emotionData)
      .catch((e) => alert(e));
  }

  stopProcessing() {
    // Clear the interval
    if (this.intervalId) {
      clearInterval(this.intervalId);
      this.intervalId = null;
    }

    // Stop the video stream
    if (this.video && this.video.srcObject) {
      const stream = this.video.srcObject;
      const tracks = stream.getTracks();
      tracks.forEach(track => track.stop());
      this.video.srcObject = null;
    }

    // Remove video element from the DOM
    if (this.video) {
      this.video.remove();
      this.video = null;
    }

    // Remove canvas element from the DOM
    if (this.canvas) {
      this.canvas.remove();
      this.canvas = null;
    }
  }
}

export default ExpressionProcessor;
