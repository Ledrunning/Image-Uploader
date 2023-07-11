<script lang="ts">
import { Options, Vue } from 'vue-class-component';
import ImageDataTable from './components/ImageDataTable.vue';

@Options({
  components: {
    ImageDataTable,
  },
})
export default class App extends Vue {}
</script>
