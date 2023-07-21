import IImageDto from "@/model/ImageDto";
import axios, { AxiosInstance, AxiosRequestConfig } from "axios";

export default class ImageApiService {
  private axiousInstance: AxiosInstance;

  constructor() {
    this.axiousInstance = axios.create({
      baseURL: "http://localhost:59871/",
    });
  }

  private async axiosCall<T>(config: AxiosRequestConfig) {
    try {
      const { data } = await this.axiousInstance.request<T>(config);
      return [null, data];
    } catch (error) {
      return [error];
    }
  }

  async getAllImages() {
    return this.axiosCall<IImageDto>({
      method: "get",
      url: `api/FileUpload/GetAll`,
    });
  }

  async getImage(imageId: number) {
    return this.axiosCall<IImageDto[]>({
      method: "get",
      url: `api/FileUpload/GetById?id=${imageId}`,
    });
  }

  async addImage(imageDto: IImageDto) {
    return this.axiosCall<IImageDto>({
      method: "put",
      url: `api/FileUpload/Create`,
      data: imageDto,
    });
  }

  //TODO: what about Id?
  async updateImage(imageId: number, imageDto: Partial<IImageDto>) {
    return this.axiosCall<IImageDto>({
      method: "put",
      url: `api/FileUpload/Update/${imageDto}`,
      data: imageDto,
    });
  }

  async deleteImage(imageId: number) {
    return this.axiosCall<void>({
      method: "delete",
      url: `api/FileUpload/Delete?id=${imageId}`,
    });
  }
}
