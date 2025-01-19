import { fileURLToPath, URL } from 'node:url'
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig(() => {

  return {
    base: "/", // 指定部署後的基路徑
    plugins: [ // 插件
      vue(),
      // vueDevTools(),
    ],
    resolve: {
      alias: {
        // 路徑簡寫
        '@': fileURLToPath(new URL('./src', import.meta.url))
      },
    },
    // 指定環境變數所在路徑
    envDir: fileURLToPath(new URL('./env', import.meta.url)),
  };
});
