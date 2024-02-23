import checkInput from "../Utils";
import ClientResponse from "./Response";

export const serverPort = "https://localhost:7172";

export async function register(
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