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


export async function initSession() {
  if (storage.getItem(isInitOccured) === null) {
    storage.setItem(isInitOccured, "true");
    fetchResponse(serverEnterAsGuest())
      .then((sessionId: string) => {
        initFields(sessionId);
        Cookies.set('session', JSON.stringify(sessionId));
        alert(Cookies.get('session'))
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
