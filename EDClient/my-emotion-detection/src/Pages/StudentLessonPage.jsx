import { createTheme } from "@mui/material/styles";
import * as faceapi from 'face-api.js';
import { useEffect } from "react";



const theme = createTheme({
  typography: {
    fontFamily: [
      "-apple-system",
      "BlinkMacSystemFont",
      '"Segoe UI"',
      "Roboto",
      '"Helvetica Neue"',
      "Arial",
      "sans-serif",
      '"Apple Color Emoji"',
      '"Segoe UI Emoji"',
      '"Segoe UI Symbol"',
    ].join(","),
  },
});

function StudentLesson() {

  let expressionsData = {
    neutral: 0,
    happy: 0,
    sad: 0,
    angry: 0,
    surprised: 0,
    disgusted: 0,
    fearful: 0
  };
  
  useEffect(() => {

    // Create video element and set attributes
    const video = document.createElement('video');
    video.setAttribute('id', 'video');
    video.setAttribute('autoplay', 'muted');
    document.body.appendChild(video);
    video.style.display = 'none';  // Set display to 'none'
    async function startFaceRecognition() {
      alert("here");
      // Load face-api.js models
    async function loadModels() {
      await faceapi.nets.tinyFaceDetector.loadFromUri('/models');
      await faceapi.nets.faceLandmark68Net.loadFromUri('/models');
      await faceapi.nets.faceRecognitionNet.loadFromUri('/models');
      await faceapi.nets.faceExpressionNet.loadFromUri('/models');
    }


      // Start video stream
      const video = document.createElement("video");
      document.body.appendChild(video);

      try {
        const stream = await navigator.mediaDevices.getUserMedia({ video: true });
        video.srcObject = stream;
        await video.play();
      } catch (error) {
        console.error(error);
      }

      // Face detection and recognition logic
      const canvas = faceapi.createCanvasFromMedia(video);
      document.body.append(canvas);

      let displaySize = { width: video.videoWidth, height: video.videoHeight };

      faceapi.matchDimensions(canvas, displaySize);

      let totalTime = 0;
      let expressionCount = 0;

      const intervalId = setInterval(async () => {
        try {
          const detections = await faceapi
            .detectAllFaces(video, new faceapi.TinyFaceDetectorOptions())
            .withFaceLandmarks()
            .withFaceExpressions();

          displaySize = { width: video.videoWidth, height: video.videoHeight };

          const resizedDetections = faceapi.resizeResults(
            detections,
            displaySize
          );

          canvas.getContext('2d').clearRect(0, 0, canvas.width, canvas.height);

          detections.forEach((detection) => {
            const expressions = detection.expressions;
            for (const expression in expressionsData) {
              expressionsData[expression] += expressions[expression];
            }
            expressionCount++;
          });

          totalTime += 100;

          if (totalTime >= 3000) {
            const averageExpressions = {};
            for (const expression in expressionsData) {
              averageExpressions[expression] =
                expressionsData[expression] / expressionCount;
            }
            console.log('Average Expressions:', averageExpressions);

            // Send the result to the server via HTTP request
            alert(averageExpressions);

            expressionsData = {
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

    startFaceRecognition();

    return () => {
      const videoElement = document.querySelector("video");
      if (videoElement) {
        const stream = videoElement.srcObject;
        const tracks = stream.getTracks();
        tracks.forEach((track) => {
          track.stop();
        });
        videoElement.srcObject = null;
      }
    };
  }, []);

  return (
    <h1>Face-API-Student</h1>
  );
}

export default StudentLesson;
