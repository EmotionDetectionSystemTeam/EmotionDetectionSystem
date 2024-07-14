import { ServiceEmotionData } from "../Objects/EmotionData";
import { serverPushEmotionData } from "./ClientService";
import { getLessonId } from "./SessionService";

const intervalTime = 300;
const frameBufferSize = 3; // Number of frames to average
const emotionThreshold = 0.75;
const processIntervalTime = 100; // Interval for processing frames in milliseconds
const minSendInterval = 5000; // Minimum interval in milliseconds to resend the same emotion

class ExpressionProcessor {
  constructor(handleExitLesson) {
    this.frameBuffer = [];
    this.video = null;
    this.canvas = null;
    this.processing = false;
    this.processInterval = null;
    this.sendInterval = null;
    this.lastSentTimes = {}; // To track the last sent time for each emotion
    this.handleExitLesson = handleExitLesson;

    // Bind methods to the instance to maintain correct context
    this.processLoop = this.processLoop.bind(this);
  }

  initializeExpressionsData() {
    return {
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
      this.video.srcObject = stream;
      await new Promise((resolve) => {
        this.video.addEventListener('loadeddata', resolve);
      });
      await this.video.play();
    } catch (error) {
      console.error('Error starting video stream:', error);
    }
  }

  async loadModels() {
    await Promise.all([
      faceapi.nets.tinyFaceDetector.loadFromUri('/models'),
      faceapi.nets.faceLandmark68Net.loadFromUri('/models'),
      faceapi.nets.faceRecognitionNet.loadFromUri('/models'),
      faceapi.nets.faceExpressionNet.loadFromUri('/models'),
    ]);
  }

  createVideoElement() {
    this.video = document.createElement('video');
    this.video.id = 'video';
    this.video.autoplay = true;
    this.video.muted = true;
    this.video.style.display = 'none';
    document.body.appendChild(this.video);
  }

  async processExpressions() {
    await this.loadModels();
    this.createVideoElement();
    await this.startVideo();

    this.canvas = faceapi.createCanvasFromMedia(this.video);
    document.body.append(this.canvas);

    this.processing = true;
    this.processInterval = setInterval(this.processLoop, processIntervalTime);
    this.sendInterval = setInterval(() => this.sendResultToServer(), intervalTime);
  }

  async processLoop() {
    if (!this.processing) return;

    try {
      const detections = await this.detectFaces();
      this.updateCanvas(detections);
    } catch (error) {
      console.error('Error processing expressions:', error);
    }
  }

  async detectFaces() {
    return await faceapi
        .detectAllFaces(this.video, new faceapi.TinyFaceDetectorOptions())
        .withFaceExpressions();
  }

  updateCanvas(detections) {
    const displaySize = { width: this.video.videoWidth, height: this.video.videoHeight };
    const resizedDetections = faceapi.resizeResults(detections, displaySize);

    this.clearCanvas();

    const frameData = this.initializeExpressionsData();
    resizedDetections.forEach((detection) => {
      const expressions = detection.expressions;
      for (const expression in expressions) {
        frameData[expression] += expressions[expression];
      }
    });

    this.addToFrameBuffer(frameData);
  }

  clearCanvas() {
    this.canvas.getContext('2d').clearRect(0, 0, this.canvas.width, this.canvas.height);
  }

  addToFrameBuffer(frameData) {
    if (this.frameBuffer.length >= frameBufferSize) {
      this.frameBuffer.shift(); // Remove the oldest frame
    }
    this.frameBuffer.push(frameData);
  }

  getAverageExpressions() {
    const averageExpressions = this.initializeExpressionsData();
    this.frameBuffer.forEach((frame) => {
      for (const emotion in frame) {
        averageExpressions[emotion] += frame[emotion];
      }
    });
    for (const emotion in averageExpressions) {
      averageExpressions[emotion] /= this.frameBuffer.length;
    }
    return averageExpressions;
  }

  sendResultToServer() {
    const averageExpressions = this.getAverageExpressions();
    const winningEmotion = this.getWinningEmotion(averageExpressions);

    if (winningEmotion && this.shouldSendEmotion(winningEmotion)) {
      console.log('Average Expressions:', averageExpressions);
      console.log('Winning Emotion:', winningEmotion);

      const emotionData = new ServiceEmotionData(winningEmotion);
      console.log("sending");
      serverPushEmotionData(getLessonId(), emotionData).catch((e) => this.handleExitLesson());
      console.log("sent");
      this.updateLastSentTime(winningEmotion);
    }
  }

  getWinningEmotion(expressionsData) {
    let maxExpression = { emotion: '', value: 0 };
    for (const [emotion, value] of Object.entries(expressionsData)) {
      if (emotion !== 'neutral' && value > emotionThreshold && value > maxExpression.value) {
        maxExpression = { emotion, value };
      }
    }
    return maxExpression.emotion ? maxExpression.emotion : null;
  }

  shouldSendEmotion(emotion) {
    const lastSentTime = this.lastSentTimes[emotion];
    if (!lastSentTime) return true;
    const now = Date.now();
    return now - lastSentTime >= minSendInterval;
  }

  updateLastSentTime(emotion) {
    this.lastSentTimes[emotion] = Date.now();
  }

  stopProcessing() {
    this.processing = false;

    this.stopVideoStream();
    this.removeElements();
    this.clearIntervals();
  }

  stopVideoStream() {
    if (this.video?.srcObject) {
      this.video.srcObject.getTracks().forEach((track) => track.stop());
      this.video.srcObject = null;
    }
  }

  removeElements() {
    this.video?.remove();
    this.video = null;

    this.canvas?.remove();
    this.canvas = null;
  }

  clearIntervals() {
    if (this.processInterval) {
      clearInterval(this.processInterval);
      this.processInterval = null;
    }
    if (this.sendInterval) {
      clearInterval(this.sendInterval);
      this.sendInterval = null;
    }
  }
}

export default ExpressionProcessor;
