import { useEffect } from "react";
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
    let intervalId;

    const startVideo = async () => {
      try {
        const stream = await navigator.mediaDevices.getUserMedia({ video: true });
        const video = document.getElementById('video');
        video.srcObject = stream;
        await video.play();
      } catch (error) {
        console.error(error);
      }
    };

    const sendResultToServer = (data) => {
      const serverUrl = 'YOUR_SERVER_ENDPOINT';
      fetch(serverUrl, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
      })
          .then(response => {
            if (!response.ok) {
              throw new Error('Network response was not ok');
            }
            return response.json();
          })
          .then(responseData => {
            console.log('Server response:', responseData);
          })
          .catch(error => {
            console.error('Error sending data to server:', error);
          });
    };

    Promise.all([
      faceapi.nets.tinyFaceDetector.loadFromUri('/models'),
      faceapi.nets.faceLandmark68Net.loadFromUri('/models'),
      faceapi.nets.faceRecognitionNet.loadFromUri('/models'),
      faceapi.nets.faceExpressionNet.loadFromUri('/models')
    ])
        .then(startVideo)
        .catch(error => console.error(error));

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

      intervalId = setInterval(async () => {
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
            for (const expression in expressionsData) {
              expressionsData[expression] += expressions[expression];
            }
            expressionCount++;
          });

          totalTime += 100;

          if (totalTime >= 30000) {
            const averageExpressions = {};
            for (const expression in expressionsData) {
              averageExpressions[expression] =
                  expressionsData[expression] / expressionCount;
            }
            console.log('Average Expressions:', averageExpressions);

            sendResultToServer(averageExpressions);

            // Clear expressions data for the next calculation
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
    });

    return () => {
      clearInterval(intervalId);
    };
  }, []);

  return (
      <h1>Face-API-Student</h1>
  );
}

export default StudentLesson;
