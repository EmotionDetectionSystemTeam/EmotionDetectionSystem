import checkInput, { serverPort } from "../Utils";
import ClientResponse from "./Response";
import { getSessionId } from "./SessionService";

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
  ): Promise<ClientResponse<string>> {
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
      return response;
    } catch (e) {
      return Promise.reject(e);
    }
  }

  export async function serverLogin(
    username: string| undefined | null,
    password: string| undefined | null
  ): Promise<ClientResponse<string>> {
    const fields: any[] = [username, password];
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
          Username: username,
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

  
      return response;
    } catch (e) {
      return Promise.reject(e);
    }
  }

