import checkInput, { serverPort } from "../Utils";
import ClientResponse from "./Response";
import { getSessionId, getUserName } from "./SessionService";

export async function serverEnterAsGuest(): Promise<ClientResponse<string>> {
    const uri = serverPort + "/api/eds/enter-as-guest";
    try {
      const jsonResponse = await fetch(uri, {
        method: "POST",
        headers: {
          accept: "application/json",
          "Content-Type": "application/json",
          "Access-Control-Allow-Origin": "*",
        },
        body: JSON.stringify({
          sessionID: "dummySession", // Pass the appropriate session ID value here
        }),
      });
  
      if (!jsonResponse.ok) {
        const errorResponse: ClientResponse<string> = await jsonResponse.json();
        alert(errorResponse.errorMessage);
      }
  
      const response = await jsonResponse.json();
  
      // Handle empty response
      if (!response) {
        throw new Error("Empty response received");
      }
  
      //const response = JSON.parse(responseText);
      return response;
    } catch (e) {
      return Promise.reject(e);
    }
  }

export async function serverRegister(
    email: string | undefined | null,
    firstName: string | undefined | null,
    lastName: string | undefined | null,
    password: string | undefined | null,
    confirmPassword: string | undefined | null,
    isStudent: number | undefined | null,
  ): Promise<string> {
    const fields: any[] = [email, firstName, lastName, password, isStudent];
    if (!checkInput(fields)) return Promise.reject("Regev's notification");
    const uri = serverPort + "/api/eds/register";
    try {
      const jsonResponse = await fetch(uri, {
        method: "POST",
        headers: {
          accept: "application/json",
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          email: email,
          firstName: firstName,
          lastName: lastName,
          password: password,
          confirmPassword: confirmPassword,
          isStudent: isStudent,
        }),
      });
      if (!jsonResponse.ok) {
        const errorResponse: ClientResponse<string> = await jsonResponse.json();
        throw new Error(errorResponse.errorMessage);
      }
  
      const response: ClientResponse<string> = await jsonResponse.json();
      // Handle empty response
      if (!response) {
        throw new Error("Empty response received");
      }
      
      //const response = JSON.parse(responseText);
      return response.value;
    } catch (e) {
      return Promise.reject(e);
    }
  }

export async function serverLogin(
    email: string| undefined | null,
    password: string| undefined | null
  ): Promise<string> {
    const fields: any[] = [email, password];
    if (!checkInput(fields)) return Promise.reject();
    const uri = serverPort + "/api/eds/login";
    try {
      const jsonResponse = await fetch(uri, {
        method: "POST",
        headers: {
          accept: "application/json",
          "Content-Type": "application/json",
        },
        // body: '{\n  "userName": "string",\n  "password": "string"\n}',
        body: JSON.stringify({
          SessionId: getSessionId(),
          Email: email,
          Password: password,
        }),
      });
  
      if (!jsonResponse.ok) {
        const errorResponse: ClientResponse<string> = await jsonResponse.json();
        throw new Error(errorResponse.errorMessage);
      }
  
      const response: ClientResponse<string> = await jsonResponse.json();
      // Handle empty response
      if (!response) {
        throw new Error("Empty response received");
      }

  
      return response.value;
    } catch (e) {
      return Promise.reject(e);
    }
  }

export async function serverJoinLesson(
    classCode: string | undefined | null,
  ): Promise<string> {
    const fields: any[] = [classCode];
    if (!checkInput(fields)) return Promise.reject();
    const uri = serverPort + "/api/eds/join-lesson";
    try {
      const jsonResponse = await fetch(uri, {
        method: "POST",
        headers: {
          accept: "application/json",
          "Content-Type": "application/json",
        },
        // body: '{\n  "userName": "string",\n  "password": "string"\n}',
        body: JSON.stringify({
          SessionId: getSessionId(),
          Email: getUserName(),
          EntryCode: classCode
        }),
      });
  
      if (!jsonResponse.ok) {
        const errorResponse: ClientResponse<string> = await jsonResponse.json();
        throw new Error(errorResponse.errorMessage);
      }
  
      const response: ClientResponse<string> = await jsonResponse.json();
      // Handle empty response
      if (!response) {
        throw new Error("Empty response received");
      }

  
      return response.value;
    } catch (e) {
      return Promise.reject(e);
    }
  }
  export async function serverCreateLesson(
    title: string | undefined | null,
    description: string | undefined | null,
    tags: string[] | undefined | null
  ): Promise<string> {
    const fields: any[] = [title,description];
    if (!checkInput(fields)) return Promise.reject();
    const uri = serverPort + "/api/eds/create-lesson";
    try {
      const jsonResponse = await fetch(uri, {
        method: "POST",
        headers: {
          accept: "application/json",
          "Content-Type": "application/json",
        },
        // body: '{\n  "userName": "string",\n  "password": "string"\n}',
        body: JSON.stringify({
          SessionId: getSessionId(),
          Email: getUserName(),
          Title: title,
          Description: description,
          Tags: tags
        }),
      });
  
      if (!jsonResponse.ok) {
        const errorResponse: ClientResponse<string> = await jsonResponse.json();
        throw new Error(errorResponse.errorMessage);
      }
  
      const response: ClientResponse<string> = await jsonResponse.json();
      // Handle empty response
      if (!response) {
        throw new Error("Empty response received");
      }

  
      return response.value;
    } catch (e) {
      return Promise.reject(e);
    }
  }

export async function serverLogout(
  ): Promise<string> {
    const uri = serverPort + "/api/eds/logout";
    try {
      const jsonResponse = await fetch(uri, {
        method: "POST",
        headers: {
          accept: "application/json",
          "Content-Type": "application/json",
        },
        // body: '{\n  "userName": "string",\n  "password": "string"\n}',
        body: JSON.stringify({
          SessionId: getSessionId(),
          Email: getUserName(),
        }),
      });
  
      if (!jsonResponse.ok) {
        const errorResponse: ClientResponse<string> = await jsonResponse.json();
        throw new Error(errorResponse.errorMessage);
      }
  
      const response: ClientResponse<string> = await jsonResponse.json();
      // Handle empty response
      if (!response) {
        throw new Error("Empty response received");
      }

  
      return response.value;
    } catch (e) {
      return Promise.reject(e);
    }
  }
