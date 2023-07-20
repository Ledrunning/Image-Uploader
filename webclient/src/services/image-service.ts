import IImageDto from "@/model/ImageDto";
import axios, { AxiosInstance, AxiosRequestConfig } from "axios";

//"api/FileUpload/GetAll"
//"api/FileUpload/GetById?id={id}"
//"api/FileUpload/Create"
//"api/FileUpload/Delete?id={id}"
//"api/FileUpload/Update"
export default class ImageApiservice {
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

  // TODO: model needed!
  async getImage(imageId: number) {
    return this.axiosCall<IImageDto>({
      method: "get",
      url: `api/FileUpload/GetById?id=${imageId}`,
    });
  }

  async updateImage(userId: number, image: Partial<IImageDto>) {
    return this.axiosCall<IImageDto>({
      method: "put",
      url: `/${userId}`,
      data: image,
    });
  }

  async deleteImage(imageId: number) {
    return this.axiosCall<void>({ method: "delete", url: `/${imageId}` });
  }
}
