export default class FileService {
  async saveImage(imageSrc: string, fileName: string) {
    // Remove 'data:image/png;base64,' from the imageSrc
    const base64Image = imageSrc.split(";base64,")[1];
    const imageBlob = this.b64toBlob(base64Image, "image/jpg");

    // Create a download link and click it
    const link = document.createElement("a");
    link.href = window.URL.createObjectURL(imageBlob);
    link.setAttribute("download", `${fileName}`);
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  private b64toBlob(b64Data: string, contentType = "", sliceSize = 512) {
    const byteCharacters = atob(b64Data);
    const byteArrays = [];

    for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
      const slice = byteCharacters.slice(offset, offset + sliceSize);

      const byteNumbers = new Array(slice.length);
      for (let i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }

      const byteArray = new Uint8Array(byteNumbers);
      byteArrays.push(byteArray);
    }

    return new Blob(byteArrays, { type: contentType });
  }
}
