<template>
  <div class="container">
    <div class="image-container">
      <img
        v-if="image"
        :src="image"
        alt="User selected image"
        class="uploaded-image"
      />
      <input
        type="file"
        @change="onFileChange"
        ref="fileInput"
        class="input-file"
        accept="image/jpeg"
      />
    </div>
    <div class="content">
      <button @click="deleteImage" class="buttons">Clear</button>
      <button @click="uploadImage" class="buttons">Add</button>
    </div>
  </div>
</template>

<script lang="ts">
import { ref } from "vue";
import ImageApiService from "@/services/image-service";
import IImageDto from "@/model/ImageDto";
import dayjs from "dayjs";
import utc from "dayjs/plugin/utc";
import timezone from "dayjs/plugin/timezone";
import { TimeType } from "@/enum/TimeType";

import "@/styles/addimage.css";

export default {
  setup() {
    const userService = new ImageApiService();
    const image = ref("");
    const fileInput = ref(null);
    const byteToMegabyteCoefficient = 0.000001;
    let selectedFile: File | null = null;
    dayjs.extend(utc);
    dayjs.extend(timezone);

    function onFileChange(e: Event) {
      const files = (e.target as HTMLInputElement).files;
      if (!files || files.length === 0) return;
      selectedFile = files[0];
      createImage(selectedFile);
    }

    function createImage(file: File) {
      const reader = new FileReader();
      reader.onload = (e) => {
        image.value = e.target?.result as string;
      };
      reader.readAsDataURL(file);
    }

    function deleteImage() {
      image.value = "";
      if (fileInput.value) {
        (fileInput.value as HTMLInputElement).value = "";
      }
      selectedFile = null;
    }

    function uploadImage() {
      if (!selectedFile) {
        alert("Please select a file first.");
        return;
      }
      const reader = new FileReader();

      reader.onload = async function (e) {
        const byteArray = new Uint8Array(e.target?.result as ArrayBuffer);
        try {
          let imageDto: IImageDto = {
            name: `MyPhoto_${getUtcDateTimeNow(TimeType.FileNameDateTime)}.jpg`,
            dateTime: dayjs().tz().toDate(),
            creationTime: selectedFile
              ? new Date(selectedFile.lastModified)
              : new Date(), // Check if selectedFile is not null
            fileSize: byteArray.byteLength * byteToMegabyteCoefficient,
            photo: Array.from(byteArray),
          };
          const response = await userService.addImage(imageDto);
          console.log(response);
        } catch (error) {
          console.log(error);
        }
      };
      reader.readAsArrayBuffer(selectedFile);
    }

    function getUtcDateTimeNow(timeType: TimeType) {
      const nowUTC = dayjs.utc();

      switch (timeType) {
        case TimeType.CurrentStandart:
          return nowUTC.format("DD-MM-YYYY HH:mm:ss");
        case TimeType.FileNameDateTime:
          return nowUTC.format("MMDDYYYY_HHmmss");
      }
    }

    return {
      image,
      onFileChange,
      deleteImage,
      uploadImage,
      fileInput,
    };
  },
};
</script>
