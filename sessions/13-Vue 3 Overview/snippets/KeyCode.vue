<template>
  <div>
    <!-- keyCode version, removed in Vue 3 -->
    <input v-on:keyup.13="submit" />

    <!-- alias version -->
    <input v-on:keyup.enter="submit" />
  </div>
</template>
