import { ServiceEmotionData } from "../Objects/EmotionData";
import { serverPushEmotionData } from "./ClientService";
import { getLessonId } from "./SessionService";

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
  }

  async startVideo() {
    try {
      const stream = await navigator.mediaDevices.getUserMedia({ video: true });
      const video = document.getElementById('video');
      video.srcObject = stream;
      await video.play();
    } catch (error) {
      console.error(error);
    }
  };

  async processExpressions() {
      await Promise.all([
        faceapi.nets.tinyFaceDetector.loadFromUri('/models'),
        faceapi.nets.faceLandmark68Net.loadFromUri('/models'),
        faceapi.nets.faceRecognitionNet.loadFromUri('/models'),
        faceapi.nets.faceExpressionNet.loadFromUri('/models'),
      ]).then(await this.startVideo());

      const video = document.createElement('video');
      video.setAttribute('id', 'video');
      video.setAttribute('autoplay', 'muted');
      document.body.appendChild(video);
      video.style.display = 'none';

      
      video.addEventListener('loadedmetadata', () => {
        const canvas = faceapi.createCanvasFromMedia(video);
        document.body.append(canvas);
  
        let displaySize = { width: video.videoWidth, height: video.videoHeight };
  
        faceapi.matchDimensions(canvas, displaySize);
  
        let totalTime = 0;
        let expressionCount = 0;
  
        this.intervalId = setInterval(async () => {
          try {
            const detections = await faceapi
              .detectAllFaces(video, new faceapi.TinyFaceDetectorOptions())
              .withFaceLandmarks()
              .withFaceDescriptors()
              .withFaceExpressions();
  
            displaySize = { width: video.videoWidth, height: video.videoHeight };
  
            const resizedDetections = faceapi.resizeResults(
              detections,
              displaySize
            );
            canvas.getContext('2d').clearRect(0, 0, canvas.width, canvas.height);
            detections.forEach((detection) => {
              const expressions = detection.expressions;
              for (const expression in this.expressionsData) {
                this.expressionsData[expression] += expressions[expression];
              }
              expressionCount++;
            });
  
            totalTime += 100;
  
            if (totalTime >= 15000) { //30000
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
      });
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
    clearInterval(this.intervalId);
  }
}

export default ExpressionProcessor;
