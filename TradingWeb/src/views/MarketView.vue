<template>
  <div class="market-view">
    <header class="market-header">
      <h1>加密货币行情</h1>
      <div class="symbol-selector">
        <label for="symbol">选择交易对：</label>
        <select id="symbol" v-model="selectedSymbol" @change="handleSymbolChange">
          <option value="BTCUSDT">BTC/USDT</option>
          <option value="ETHUSDT">ETH/USDT</option>
          <option value="BNBUSDT">BNB/USDT</option>
          <option value="SOLUSDT">SOL/USDT</option>
          <option value="ADAUSDT">ADA/USDT</option>
        </select>
      </div>
    </header>

    <section class="price-info">
      <div class="price-item">
        <span class="label">当前价格：</span>
        <span class="price" :class="{ 'price-up': currentSnapshot.bidPrice > currentSnapshot.askPrice, 'price-down': currentSnapshot.bidPrice < currentSnapshot.askPrice }">
          ${{ currentSnapshot.price.toFixed(2) }}
        </span>
      </div>
      <div class="price-item">
        <span class="label">标记价格：</span>
        <span class="mark-price">${{ currentSnapshot.markPrice.toFixed(2) }}</span>
      </div>
      <div class="price-item">
        <span class="label">指数价格：</span>
        <span class="index-price">${{ currentSnapshot.indexPrice.toFixed(2) }}</span>
      </div>
      <div class="price-item">
        <span class="label">24h最高：</span>
        <span class="high">${{ currentSnapshot.high.toFixed(2) }}</span>
      </div>
      <div class="price-item">
        <span class="label">24h最低：</span>
        <span class="low">${{ currentSnapshot.low.toFixed(2) }}</span>
      </div>
      <div class="price-item">
        <span class="label">24h成交量：</span>
        <span class="volume">{{ currentSnapshot.volume.toFixed(2) }}</span>
      </div>
    </section>

    <section class="order-book">
      <div class="order-book-item">
        <span class="label">买一价：</span>
        <span class="bid-price">${{ currentSnapshot.bidPrice.toFixed(2) }}</span>
        <span class="bid-qty">{{ currentSnapshot.bidQty.toFixed(4) }}</span>
      </div>
      <div class="order-book-item">
        <span class="label">卖一价：</span>
        <span class="ask-price">${{ currentSnapshot.askPrice.toFixed(2) }}</span>
        <span class="ask-qty">{{ currentSnapshot.askQty.toFixed(4) }}</span>
      </div>
    </section>

    <section class="chart-container">
      <div class="interval-selector">
        <button 
          v-for="interval in intervals" 
          :key="interval.value" 
          :class="{ 'active': selectedInterval === interval.value }" 
          @click="handleIntervalChange(interval.value)"
        >
          {{ interval.label }}
        </button>
      </div>
      <KLineChart :data="klineData" :symbol="selectedSymbol" :interval="selectedInterval" />
    </section>

    <footer class="market-footer">
      <div class="signalr-status" :class="{ 'connected': isSignalRConnected, 'disconnected': !isSignalRConnected }">
        SignalR连接状态：{{ isSignalRConnected ? '已连接' : '未连接' }}
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, watch } from 'vue';
import KLineChart from '../components/KLineChart/index.vue';
import { useSignalR } from '../hooks/useSignalR';
import { getKLineData, getMarketSnapshot, handleRealTimeUpdate } from '../services/marketService';

interface IntervalOption {
  label: string;
  value: string;
}

// 时间周期选项
const intervals: IntervalOption[] = [
  { label: '1分钟', value: '1m' },
  { label: '5分钟', value: '5m' },
  { label: '15分钟', value: '15m' },
  { label: '30分钟', value: '30m' },
  { label: '1小时', value: '1h' },
  { label: '4小时', value: '4h' },
  { label: '1天', value: '1d' }
];

// 状态管理
const selectedSymbol = ref('BTCUSDT');
const selectedInterval = ref('1m');
const klineData = ref<any[]>([]);
const currentSnapshot = ref({
  symbol: '',
  markPrice: 0,
  indexPrice: 0,
  bidPrice: 0,
  bidQty: 0,
  askPrice: 0,
  askQty: 0,
  updateTime: '',
  price: 0,
  change: 0,
  changePercent: 0,
  high: 0,
  low: 0,
  volume: 0
});

// 使用SignalR hook
const { 
  isConnected: isSignalRConnected, 
  subscribeSymbol, 
  unsubscribeSymbol 
} = useSignalR();

// 加载KLine数据
const loadKLineData = async () => {
  const data = await getKLineData(selectedSymbol.value, selectedInterval.value, 100);
  klineData.value = data;
};

// 加载行情快照
const loadMarketSnapshot = async () => {
  const snapshot = await getMarketSnapshot(selectedSymbol.value);
  // 更新当前行情快照，处理可选属性
  currentSnapshot.value = {
    ...currentSnapshot.value,
    ...snapshot,
    change: snapshot.change || 0,
    changePercent: snapshot.changePercent || 0,
    high: snapshot.high || 0,
    low: snapshot.low || 0,
    volume: snapshot.volume || 0
  };
};

// 处理交易对变化
const handleSymbolChange = async () => {
  // 取消订阅之前的交易对
  await unsubscribeSymbol(selectedSymbol.value);
  // 加载新交易对的数据
  await Promise.all([
    loadKLineData(),
    loadMarketSnapshot()
  ]);
  // 订阅新的交易对
  await subscribeSymbol(selectedSymbol.value);
};

// 处理时间周期变化
const handleIntervalChange = async (interval: string) => {
  selectedInterval.value = interval;
  await loadKLineData();
};

// 处理实时行情更新
const handleSymbolUpdate = (event: CustomEvent) => {
  const snapshot = event.detail;
  if (snapshot.symbol === selectedSymbol.value) {
    // 更新当前行情快照，处理可选属性
    currentSnapshot.value = {
      ...currentSnapshot.value,
      ...snapshot,
      change: snapshot.change || 0,
      changePercent: snapshot.changePercent || 0,
      high: snapshot.high || 0,
      low: snapshot.low || 0,
      volume: snapshot.volume || 0
    };
    // 更新KLine数据
    klineData.value = handleRealTimeUpdate(snapshot, klineData.value);
  }
};

// 监听SignalR连接状态变化
watch(isSignalRConnected, async (newStatus) => {
  if (newStatus) {
    // 连接成功后订阅当前交易对
    await subscribeSymbol(selectedSymbol.value);
  }
});

// 生命周期钩子
onMounted(async () => {
  // 加载初始数据
  await Promise.all([
    loadKLineData(),
    loadMarketSnapshot()
  ]);
  
  // 订阅实时行情更新事件
  window.addEventListener('SymbolUpdate', handleSymbolUpdate as EventListener);
});

onUnmounted(() => {
  // 取消订阅实时行情更新事件
  window.removeEventListener('SymbolUpdate', handleSymbolUpdate as EventListener);
  // 取消订阅SignalR
  unsubscribeSymbol(selectedSymbol.value);
});
</script>

<style scoped>
.market-view {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
  font-family: Arial, sans-serif;
}

.market-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.market-header h1 {
  margin: 0;
  color: #333;
}

.symbol-selector {
  display: flex;
  align-items: center;
  gap: 10px;
}

.symbol-selector select {
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 16px;
}

.price-info {
  display: flex;
  gap: 20px;
  margin-bottom: 20px;
  padding: 15px;
  background-color: #f5f5f5;
  border-radius: 8px;
  flex-wrap: wrap;
}

.price-item {
  display: flex;
  align-items: center;
  gap: 5px;
}

.label {
  font-weight: bold;
  color: #666;
}

.price {
  font-size: 24px;
  font-weight: bold;
}

.price-up {
  color: #ef5350;
}

.price-down {
  color: #26a69a;
}

.change {
  font-weight: bold;
}

.change-up {
  color: #ef5350;
}

.change-down {
  color: #26a69a;
}

.high {
  color: #ef5350;
  font-weight: bold;
}

.low {
  color: #26a69a;
  font-weight: bold;
}

.volume {
  font-weight: bold;
  color: #333;
}

.chart-container {
  margin-bottom: 20px;
}

.interval-selector {
  display: flex;
  gap: 10px;
  margin-bottom: 15px;
}

.interval-selector button {
  padding: 8px 16px;
  border: 1px solid #ddd;
  border-radius: 4px;
  background-color: #fff;
  cursor: pointer;
  transition: all 0.3s ease;
}

.interval-selector button:hover {
  background-color: #f0f0f0;
}

.interval-selector button.active {
  background-color: #1976d2;
  color: white;
  border-color: #1976d2;
}

.market-footer {
  padding: 10px;
  background-color: #f5f5f5;
  border-radius: 8px;
  text-align: center;
}

.signalr-status {
  padding: 5px 10px;
  border-radius: 4px;
  font-weight: bold;
}

.signalr-status.connected {
  background-color: #e8f5e8;
  color: #2e7d32;
}

.signalr-status.disconnected {
  background-color: #ffebee;
  color: #c62828;
}

.order-book {
  display: flex;
  gap: 20px;
  margin-bottom: 20px;
  padding: 15px;
  background-color: #f5f5f5;
  border-radius: 8px;
  flex-wrap: wrap;
}

.order-book-item {
  display: flex;
  align-items: center;
  gap: 10px;
}

.bid-price {
  color: #26a69a;
  font-weight: bold;
}

.bid-qty {
  color: #333;
  font-weight: bold;
}

.ask-price {
  color: #ef5350;
  font-weight: bold;
}

.ask-qty {
  color: #333;
  font-weight: bold;
}

.mark-price {
  color: #1976d2;
  font-weight: bold;
}

.index-price {
  color: #ff9800;
  font-weight: bold;
}

/* 响应式设计 */
@media (max-width: 768px) {
  .price-info {
    flex-direction: column;
    gap: 10px;
  }
  
  .order-book {
    flex-direction: column;
    gap: 10px;
  }
  
  .interval-selector {
    flex-wrap: wrap;
  }
}
</style>