<script>
export default {
  props: {
    value: String,
  },
  methods: {
    updateValue() {
      this.$emit('input', 'updated value');
    },
  },
};
</script>
