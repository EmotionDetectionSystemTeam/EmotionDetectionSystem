import Cookies from 'js-cookie';
import { serverEnterAsGuest } from "./ClientService";
import { fetchResponse } from "./GeneralService";


interface Iuser {
  username: string;
}

export const storage = sessionStorage;
const isInitOccured = "isInitOccured";
const userName = "userName";
const isGuest = "isGuest";
const sessionId = "sessionId";
const isAdmin = "isAdmin";
const LessonId = "LessonId";


export async function initSession() {
  if (storage.getItem(isInitOccured) === null) {
    storage.setItem(isInitOccured, "true");
    fetchResponse(serverEnterAsGuest())
      .then((sessionId: string) => {
        initFields(sessionId);
        //initializeCookie(sessionId);
        alert(sessionId);
      })
      .catch((e) => {
        alert("Sorry, Could not enter the server");
      });
  }
}

export async function initFields(id: string) {
  storage.setItem(isInitOccured, "true");
  setSessionId(id);
  setIsGuest(true);
  //setUsername("guest");
  setIsAdmin(false);
}

export function clearSession() {
  storage.clear();
}

export function getIsInitOccured(): boolean {
  const value = storage.getItem(isInitOccured);
  return value === "true";
}

export function setIsInitOccured(value: boolean): void {
  storage.setItem(isInitOccured, value.toString());
}

export function getUserName(): string | null {
  return storage.getItem(userName);
}

export function setUsername(value: string): void {
  storage.setItem(userName, value);
}

export function getIsGuest(): boolean {
  const value = storage.getItem(isGuest);
  return value === "true";
}

export function setIsGuest(value: boolean): void {
  storage.setItem(isGuest, value.toString());
}

export function getSessionId(): string | null {
  return storage.getItem(sessionId);
}

export function setSessionId(value: string): void {
  storage.setItem(sessionId, value);
}

export function getIsAdmin(): boolean {
  const value = storage.getItem(isAdmin);
  return value === "true";
}

export function setIsAdmin(value: boolean): void {
  storage.setItem(isAdmin, value.toString());
}

export function getLessonId(): string | null {
  return storage.getItem(LessonId);
}

export function setLessonId(value: string): void {
  storage.setItem(LessonId, value);
}

export const initializeCookie = (sessionId) => {
  
  const cookieName = `session_${sessionId}`; // Construct the cookie name
  Cookies.set(cookieName, null, { expires: 1 }); // expires in 1 day

  /*
  const existingData = Cookies.getJSON(cookieName); // Retrieve existing data

  // If no data exists for the cookie, initialize it with an empty object
  if (!existingData) {
      Cookies.set(cookieName, {}, { expires: 1 }); // expires in 1 day
  }
  */
};

// Function to set a cookie
export const setCookie = (sessionId, key, value) => {
  const cookieName = `session_${sessionId}`; // Construct the cookie name
  const existing = Cookies.get(cookieName); // Retrieve existing data or initialize an empty object
  const existingData = existing ? JSON.parse(existing) : {}
  // Update the existing data with the new key-value pair
  existingData[key] = value;

  // Set the cookie with the updated data
  Cookies.set(cookieName, JSON.stringify(existingData), { expires: 1 }); // expires in 1 day
};

// Function to get a value from the cookie
export const getCookie = (sessionId, key) => {
  const cookieName = `session_${sessionId}`; // Construct the cookie name
  const cookieMenu = Cookies.get(cookieName); // Retrieve the cookie data
    //const lesson : Lesson = lessonCookie ? JSON.parse(lessonCookie) : null;
  const cookieData = cookieMenu ? JSON.parse(cookieMenu) : null;

  // If the cookie data exists and has the specified key, return its value
  if (cookieData && cookieData.hasOwnProperty(key)) {
      return cookieData[key];
  }

  // If the key doesn't exist or cookieData is null, return null
  return null;
};