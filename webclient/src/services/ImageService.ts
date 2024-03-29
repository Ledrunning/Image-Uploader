import IImageDto from "@/model/ImageDto";
import IImageLoadDto from "@/model/ImageLoadDto";
import axios, { AxiosInstance, AxiosRequestConfig } from "axios";
import { getConfig } from "@/services/ConfigLoader";
import IConfig from "@/model/Config";

export default class ImageApiService {
  private axiousInstance: AxiosInstance;
  private config: IConfig;

  constructor() {
    // Load the configuration
    this.config = getConfig();

    this.axiousInstance = axios.create({
      baseURL: this.config.apiUrl,
    });
    axios.defaults.headers.post["Content-Type"] = "application/json";
  }

  async axiosCall<T>(config: AxiosRequestConfig): Promise<T> {
    const response = await this.axiousInstance.request<T>(config);
    return response.data;
  }

  async getAllImages(): Promise<IImageLoadDto[]> {
    return this.axiosCall<IImageLoadDto[]>({
      method: "get",
      url: `api/FileUpload/GetAll`,
    });
  }

  async getImage(imageId: number) {
    return this.axiosCall<IImageDto>({
      method: "get",
      url: `api/FileUpload/GetById?id=${imageId}`,
    });
  }

  async addImage(imageDto: IImageDto) {
    return this.axiosCall<IImageDto>({
      method: "post",
      url: `api/FileUpload/Create`,
      data: imageDto,
    });
  }

  async updateImage(imageDto: IImageDto) {
    return this.axiosCall<IImageDto>({
      method: "post",
      url: "api/FileUpload/Update/",
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
