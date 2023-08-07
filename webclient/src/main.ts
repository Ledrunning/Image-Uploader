import { createApp } from "vue";
import App from "./App.vue";
import "./registerServiceWorker";
import router from "./router";
import Vue3EasyDataTable from "vue3-easy-data-table";

import "vue3-easy-data-table/dist/style.css";

createApp(App)
  .component("EasyDataTable", Vue3EasyDataTable)
  .use(router)
  .mount("#app");
