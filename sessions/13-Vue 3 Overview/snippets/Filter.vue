<template>
  <h1>Bank Account Balance</h1>
  <p>{{ accountBalance | currencyUSD }}</p>
</template>

<script>
export default {
  props: {
    accountBalance: {
      type: Number,
      required: true,
    },
  },
  filters: {
    currencyUSD(value) {
      return '$' + value;
    },
  },
};
</script>
