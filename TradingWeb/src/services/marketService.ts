import axios from 'axios';

// API基础URL
const API_BASE_URL = '/api/Market';

// 创建axios实例
const apiClient = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000
});

// K线数据接口，匹配后端Candlestick实体
interface KLineData {
  time: number;
  open: number;
  close: number;
  high: number;
  low: number;
  volume: number;
}

// 行情快照接口，匹配后端FuturesMarketSnapshot模型
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
  change?: number;
  changePercent?: number;
  high?: number;
  low?: number;
  volume?: number;
}

// 后端Candlestick实体结构
interface CandlestickEntity {
  id: number;
  symbol: string;
  interval: string;
  openTime: number;
  open: number;
  high: number;
  low: number;
  close: number;
  volume: number;
  closeTime: number;
  quoteAssetVolume: number;
  numberOfTrades: number;
  takerBuyBaseAssetVolume: number;
  takerBuyQuoteAssetVolume: number;
  // 其他字段...
}

// 获取历史KLine数据
export const getKLineData = async (symbol: string, interval: string, limit: number = 100): Promise<KLineData[]> => {
  try {
    const response = await apiClient.get(`/candlestick/${symbol}/${interval}`, {
      params: { limit }
    });
    
    // 将后端返回的Candlestick实体转换为前端需要的KLineData格式
    const candlesticks: CandlestickEntity[] = response.data;
    return candlesticks.map(item => ({
      time: item.openTime,
      open: Number(item.open),
      close: Number(item.close),
      high: Number(item.high),
      low: Number(item.low),
      volume: Number(item.volume)
    }));
  } catch (error) {
    console.error('Failed to get KLine data:', error);
    // 出错时返回空数组
    return [];
  }
};

// 获取当前行情快照
export const getMarketSnapshot = async (symbol: string): Promise<MarketSnapshot> => {
  try {
    const response = await apiClient.get(`/ticker/${symbol}`);
    const snapshot: any = response.data;
    
    // 将后端返回的数据转换为前端需要的格式
    return {
      ...snapshot,
      price: Number(snapshot.markPrice), // 使用标记价作为当前价格
      high: 0, // 后续可以从其他接口获取
      low: 0,  // 后续可以从其他接口获取
      volume: 0 // 后续可以从其他接口获取
    };
  } catch (error) {
    console.error(`Failed to get market snapshot for ${symbol}:`, error);
    // 出错时返回默认值
    return {
      symbol,
      markPrice: 0,
      indexPrice: 0,
      bidPrice: 0,
      bidQty: 0,
      askPrice: 0,
      askQty: 0,
      updateTime: new Date().toISOString(),
      price: 0
    };
  }
};

// 获取所有交易对行情快照
export const getAllMarketSnapshots = async (): Promise<MarketSnapshot[]> => {
  try {
    const response = await apiClient.get(`/tickers`);
    const snapshots: any[] = response.data;
    
    // 将后端返回的数据转换为前端需要的格式
    return snapshots.map(item => ({
      ...item,
      price: Number(item.markPrice)
    }));
  } catch (error) {
    console.error('Failed to get all market snapshots:', error);
    // 出错时返回空数组
    return [];
  }
};

// 处理实时行情更新
export const handleRealTimeUpdate = (snapshot: MarketSnapshot, klineData: KLineData[]): KLineData[] => {
  if (klineData.length === 0) {
    // 如果没有数据，返回空数组
    return klineData;
  }

  const now = Date.now();
  const lastKLine = klineData[klineData.length - 1];
  if (!lastKLine) {
    return klineData;
  }

  // 计算K线周期的毫秒数
  const getPeriodMilliseconds = (interval: string): number => {
    switch (interval.toLowerCase()) {
      case '1m': return 60 * 1000;
      case '5m': return 5 * 60 * 1000;
      case '15m': return 15 * 60 * 1000;
      case '30m': return 30 * 60 * 1000;
      case '1h': return 60 * 60 * 1000;
      case '4h': return 4 * 60 * 60 * 1000;
      case '1d': return 24 * 60 * 60 * 1000;
      default: return 60 * 1000;
    }
  };

  // 假设当前K线数据的周期是1分钟，实际项目中应该根据实际情况调整
  const interval = getPeriodMilliseconds('1m');

  // 检查是否需要创建新的KLine
  if (now - lastKLine.time >= interval) {
    // 创建新的KLine
    const newKLine: KLineData = {
      time: now,
      open: snapshot.price,
      close: snapshot.price,
      high: snapshot.price,
      low: snapshot.price,
      volume: 0
    };
    return [...klineData.slice(1), newKLine];
  } else {
    // 更新当前KLine
    const updatedKLineData = [...klineData];
    const currentKLine = updatedKLineData[updatedKLineData.length - 1];
    if (currentKLine) {
      updatedKLineData[updatedKLineData.length - 1] = {
        time: currentKLine.time,
        open: currentKLine.open,
        close: snapshot.price,
        high: Math.max(currentKLine.high, snapshot.price),
        low: Math.min(currentKLine.low, snapshot.price),
        volume: currentKLine.volume // 实际项目中应该累加成交量
      };
    }
    return updatedKLineData;
  }
};