import axios, { AxiosInstance, AxiosRequestConfig } from "axios";

export default class ImageApiservice {
  private axiousInstance: AxiosInstance;

  constructor() {
    this.axiousInstance = axios.create({
      baseURL: "https://xyz.com",
    });
  }
}

private async axiousCall<T>(config: AxiosRequestConfig) {
    try{
        const { data } = await this.axiousInstance.request<T>(config);
        return [null, data];
    } catch (error) {
        return [error];
    }
}