<script>
export default {
  /******* Vue 2 *******/
  beforeDestroy() {
    console.log('Component will be destroyed');
  },
  destroyed() {
    console.log('Component has been destroyed');
  },

  /******* Vue 3 *******/
  beforeUnmount() {
    console.log('Component will be destroyed');
  },
  unmounted() {
    console.log('Component has been destroyed');
  },
};
</script>
