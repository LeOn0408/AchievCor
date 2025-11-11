<template>
  <div class="not-found-page">
    <div class="container">
      <div class="error-code">404</div>
      <h1 class="error-title">Страница не найдена</h1>
      <p class="error-description">
        Запрашиваемая страница не существует или была перемещена.
      </p>
      <router-link to="/" class="home-button">
        На главную
      </router-link>

      <div class="divider"></div>

      <p class="help-text">
        Если вы считаете, что это ошибка,<br>
        обратитесь в техническую поддержку.
      </p>
    </div>
  </div>
</template>

<style scoped>
  .not-found-page {
    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
    color: #334155;
    line-height: 1.6;
    min-height: 100vh;
    display: flex;
    align-items: center;
    justify-content: center;
    padding: 1rem;
  }

  .error-code {
    font-size: 4rem;
    font-weight: 300;
    color: #64748b;
    margin-bottom: 1rem;
  }

  .error-title {
    font-size: 1.5rem;
    font-weight: 600;
    color: #1e293b;
    margin-bottom: 1rem;
  }

  .error-description {
    color: #64748b;
    margin-bottom: 2rem;
  }

  .home-button {
    display: inline-block;
    padding: 0.75rem 1.5rem;
    background-color: #3b82f6;
    color: white;
    text-decoration: none;
    border-radius: 6px;
    font-weight: 500;
    transition: background-color 0.2s;
    border: none;
    cursor: pointer;
  }

    .home-button:hover {
      background-color: #2563eb;
    }

  .divider {
    height: 1px;
    background-color: #e2e8f0;
    margin: 2rem 0;
  }

  .help-text {
    font-size: 0.875rem;
    color: #94a3b8;
  }
</style>
