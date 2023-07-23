import { FileUpdate } from "@/enum/FileUpdate";

export default interface IImageDto {
  id?: number;
  name: string;
  dateTime: Date;
  creationTime: Date;
  fileSize: number;
  photo: Uint8Array[];
  photoPath: string;
  lastPhotoName: string;
  fileUpdate: FileUpdate;
}
