import ClientResponse from "./Response";

export async function fetchResponse<T>(
  responsePromise: Promise<ClientResponse<T>>
) {
  try {
    const serverResponse = await responsePromise;
    if (serverResponse.errorOccured) {
      return Promise.reject(serverResponse.errorMessage);
    }
    return serverResponse.value;
  } catch (e) {
    return Promise.reject("An error occured trying to send your request");
  }
}
