import { createApp } from 'vue'
import App from './App.vue'
import ImageDataTable from 'vue3-easy-data-table';
import 'vue3-easy-data-table/dist/style.css';

const app = createApp(App);
app.component('EasyDataTable', ImageDataTable);
