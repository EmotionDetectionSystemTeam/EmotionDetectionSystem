import { ServiceEmotionData } from "../Objects/EmotionData";
import { serverPushEmotionData } from "./ClientService";
import { getLessonId } from "./SessionService";

const intervalTime = 4000;

class ExpressionProcessor {
  constructor() {
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
    this.processing = false;
    this.lastProcessTime = 0;
    this.timer = null; // Background timer to periodically send data to server

    // Bind methods to the instance to maintain correct context
    this.processLoop = this.processLoop.bind(this);
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

    // Start the processing loop
    this.processing = true;
    this.processLoop();

    // Start background timer to periodically send data to server
    this.timer = setInterval(() => {
      this.sendResultToServer();
    }, intervalTime);
  }

  async processLoop() {
    if (!this.processing) return;

    try {
      const detections = await faceapi
          .detectAllFaces(this.video, new faceapi.TinyFaceDetectorOptions())
          .withFaceLandmarks()
          .withFaceDescriptors()
          .withFaceExpressions();

      // Update display size
      let displaySize = { width: this.video.videoWidth, height: this.video.videoHeight };

      const resizedDetections = faceapi.resizeResults(detections, displaySize);
      this.canvas.getContext('2d').clearRect(0, 0, this.canvas.width, this.canvas.height);

      // Reset expressions data for accurate accumulation
      Object.keys(this.expressionsData).forEach((expression) => {
        this.expressionsData[expression] = 0;
      });

      // Accumulate expressions data from detections
      detections.forEach((detection) => {
        const expressions = detection.expressions;
        for (const expression in expressions) {
          this.expressionsData[expression] += expressions[expression];
        }
      });

      // Check visibility state to decide animation frame or timeout
      if (document.visibilityState === 'visible') {
        requestAnimationFrame(() => this.processLoop());
      } else {
        setTimeout(() => this.processLoop(), 1000); // Adjust timeout as needed
      }

    } catch (error) {
      console.error(error);
    }
  }


  async sendResultToServer() {
    const emotionData = new ServiceEmotionData(
        this.expressionsData.neutral,
        this.expressionsData.happy,
        this.expressionsData.sad,
        this.expressionsData.angry,
        this.expressionsData.surprised,
        this.expressionsData.disgusted,
        this.expressionsData.fearful
    );

    // Send data to server
    serverPushEmotionData(getLessonId(), emotionData)
        .then(() => {
          // Clear expressions data after successful send
          Object.keys(this.expressionsData).forEach((expression) => {
            this.expressionsData[expression] = 0;
          });
        })
        .catch((e) => console.error("Error sending data to server:", e));
  }

  stopProcessing() {
    this.processing = false;

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

    // Clear the background timer
    if (this.timer) {
      clearInterval(this.timer);
      this.timer = null;
    }
  }
}

export default ExpressionProcessor;
