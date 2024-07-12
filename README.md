# Emotion Detection System

The Emotion Detection System is a real-time application that uses the `face-api.js` library to detect facial expressions from a video stream and send emotion data to a server at regular intervals. This system is designed to handle video input, process facial expressions, and transmit the detected emotions efficiently.

## Table of Contents
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Scripts](#scripts)
- [Contributing](#contributing)

## Goal
The goal of the Emotion Detection System is to detect students' expressions during lessons in real-time while in class and send them to the teacher who opened the class. This allows the teacher to handle negative emotions in real-time during exercises, ensuring a better learning environment.

## Features
* **Real-Time Facial Expression Detection**:
    - Detects student's facial expressions in real-time during lessons using `face-api.js`.
    - Provides immediate feedback to teachers on students' emotional states.

* **Teacher Dashboard**:
    - Offers a dashboard for teachers to monitor analyzed facial expressions of students during lessons.
    - Facilitates prompt response to students displaying negative emotions.

* **Student Enrollment and Authentication**:
    - Enables students to enroll in classes using unique class codes.
    - Ensures secure authentication and access to lessons with camera activation permissions.

* **Aggregate Emotional Data Analysis**:
    - Provides teachers with aggregate data on students' emotional states across multiple lessons.
    - Supports insights into emotional trends and patterns to enhance classroom dynamics and student support.

* **Systematic Lesson Integration**:
    - Integrates seamlessly into lessons, activating facial expression detection upon students entering the class.
    - Ensures smooth operation and minimal disruption to the learning environment.

* **User-Friendly Interface**:
    - Offers an intuitive interface for both teachers and students, simplifying class management and participation.

* **Privacy and Security**:
    - Ensures student privacy by activating cameras only during lessons and for facial expression analysis.
    - Implements robust security measures to protect student data and system integrity.

## Installation
1. Clone the repository:
   ```sh
   git clone https://github.com/EmotionDetectionSystemTeam/EmotionDetectionSystem.git
   ```
2. Navigate to the project directory:
    ```sh
    cd EmotionDetectionSystem
    ```
3. Install the required dependencies:
    ```sh
    npm install
    ```
4. Run EmotionDetectionServer from the IDE.
5. Nevigate to my-emotion-detection folder:
    ```sh
    cd EDClient/my-emotion-detection
    ```
6. Run the client:
    * Update the server URL in the `src/Utils.tsx` file to your local server.
    * Run the client:
        ```sh
        npm run dev
        ```
## Scripts
`npm build`: Build the project for production.
`npm run dev`: Run the project in development mode.

## Usage
1. **Teacher Registration and Class Creation**:
    - The teacher registers and creates a class within the Emotion Detection System.
    - The system generates a unique class code which is shared with the students.

2. **Student Enrollment**:
    - Students receive the class code from the teacher.
    - Students use the class code to enroll in the class through the system.

3. **Lesson Participation**:
    - Students log in to the system and enter the class using the provided class code.
    - Upon entering the lesson, students grant permission to activate their camera for facial expression analysis.

4. **Facial Expression Analysis**:
    - The system activates the camera and begins real-time facial expression detection using the `face-api.js` model.
    - Detected facial expressions are analyzed and sent to the teacher's dashboard at regular intervals during the lesson.

5. **Teacher Monitoring**:
    - The teacher monitors the students' analyzed facial expressions in real-time via the system's dashboard.
    - Teachers can respond promptly to students showing negative emotions, ensuring a supportive learning environment.

6. **Aggregate Data Analysis**:
    - Teachers can access aggregate data on the emotions of students enrolled in their classes over multiple sessions.
    - The system provides insights into trends and patterns of student emotions during lessons, facilitating better understanding and support strategies.

7. **End of Lesson**:
    - At the end of the lesson, the camera deactivates, and students exit the lesson session.

## Contributing
Contributions to the Emotion Detection System are welcome! To contribute, please follow these steps:
1. Fork the repository.
2. Create a new branch for your feature or bug fix:
    ```sh
    git checkout -b feature/your-feature-name
    ```
3. Make your changes and commit them:
    ```sh
    git commit -m "Your commit message"
    ```
4. Push your changes to your fork:
    ```sh
    git push origin feature/your-feature-name
    ```
5. Create a pull request to the main repository.
