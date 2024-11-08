<script>
export default {
  props: {
    modelValue: String,
  },
  methods: {
    updateValue() {
      this.$emit('update:modelValue', 'updated value');
    },
  },
};
</script>
