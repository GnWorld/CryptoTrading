import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { ref, onMounted, onUnmounted } from 'vue';

// 行情快照接口，与后端FuturesMarketSnapshot模型匹配
interface MarketSnapshot {
  symbol: string;
  markPrice: number;
  indexPrice: number;
  bidPrice: number;
  bidQty: number;
  askPrice: number;
  askQty: number;
  updateTime: string;
  // 计算字段
  price: number;
}

export function useSignalR() {
  const connection = ref<HubConnection | null>(null);
  const isConnected = ref(false);
  const error = ref<string | null>(null);

  // 建立连接
  const connect = async () => {
    try {
      const hubConnection = new HubConnectionBuilder()
        .withUrl('/MarketHub') // 后端MarketHub的URL
        .withAutomaticReconnect()
        .configureLogging(LogLevel.Information)
        .build();

      // 连接事件处理
      hubConnection.on('Connected', () => {
        console.log('SignalR connected');
        isConnected.value = true;
      });

      hubConnection.on('Disconnected', () => {
        console.log('SignalR disconnected');
        isConnected.value = false;
      });

      // 接收单个交易对的行情更新
      hubConnection.on('SymbolUpdate', (data: MarketSnapshot) => {
        console.log('SymbolUpdate:', data);
        // 转换数据格式，确保包含price字段
        const formattedData = {
          ...data,
          price: data.markPrice // 使用markPrice作为当前价格
        };
        // 触发价格更新事件
        window.dispatchEvent(new CustomEvent('SymbolUpdate', { detail: formattedData }));
      });

      // 接收所有交易对的行情更新
      hubConnection.on('AllMarketUpdate', (data: MarketSnapshot[]) => {
        console.log('AllMarketUpdate:', data);
        // 转换数据格式
        const formattedData = data.map(item => ({
          ...item,
          price: item.markPrice
        }));
        // 触发全市场更新事件
        window.dispatchEvent(new CustomEvent('AllMarketUpdate', { detail: formattedData }));
      });

      await hubConnection.start();
      connection.value = hubConnection;
      isConnected.value = true;
      error.value = null;
    } catch (err) {
      console.error('SignalR connection error:', err);
      error.value = err instanceof Error ? err.message : 'Failed to connect to SignalR';
      isConnected.value = false;
    }
  };

  // 断开连接
  const disconnect = async () => {
    if (connection.value) {
      await connection.value.stop();
      connection.value = null;
      isConnected.value = false;
    }
  };

  // 订阅单个交易对
  const subscribeSymbol = async (symbol: string) => {
    if (connection.value && isConnected.value) {
      try {
        await connection.value.invoke('SubscribeSymbol', symbol);
        console.log(`Subscribed to symbol: ${symbol}`);
      } catch (err) {
        console.error(`Failed to subscribe to ${symbol}:`, err);
      }
    }
  };

  // 取消订阅单个交易对
  const unsubscribeSymbol = async (symbol: string) => {
    if (connection.value && isConnected.value) {
      try {
        await connection.value.invoke('UnsubscribeSymbol', symbol);
        console.log(`Unsubscribed from symbol: ${symbol}`);
      } catch (err) {
        console.error(`Failed to unsubscribe from ${symbol}:`, err);
      }
    }
  };

  // 订阅所有交易对
  const subscribeAllMarket = async () => {
    if (connection.value && isConnected.value) {
      try {
        await connection.value.invoke('SubscribeAllMarket');
        console.log('Subscribed to all market');
      } catch (err) {
        console.error('Failed to subscribe to all market:', err);
      }
    }
  };

  // 取消订阅所有交易对
  const unsubscribeAllMarket = async () => {
    if (connection.value && isConnected.value) {
      try {
        await connection.value.invoke('UnsubscribeAllMarket');
        console.log('Unsubscribed from all market');
      } catch (err) {
        console.error('Failed to unsubscribe from all market:', err);
      }
    }
  };

  // 生命周期钩子
  onMounted(() => {
    connect();
  });

  onUnmounted(() => {
    disconnect();
  });

  return {
    connection,
    isConnected,
    error,
    connect,
    disconnect,
    subscribeSymbol,
    unsubscribeSymbol,
    subscribeAllMarket,
    unsubscribeAllMarket
  };
}