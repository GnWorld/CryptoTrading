<template>
  <div ref="chartRef" class="kline-chart"></div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, watch, nextTick } from 'vue';
import * as echarts from 'echarts';

interface KLineData {
  time: number; // 时间戳
  open: number; // 开盘价
  close: number; // 收盘价
  high: number; // 最高价
  low: number; // 最低价
  volume: number; // 成交量
}

interface Props {
  data: KLineData[];
  symbol: string;
  interval?: string;
}

const props = withDefaults(defineProps<Props>(), {
  interval: '1m'
});

const chartRef = ref<HTMLElement | null>(null);
let chartInstance: echarts.ECharts | null = null;

// 初始化图表
const initChart = () => {
  if (!chartRef.value) return;

  chartInstance = echarts.init(chartRef.value);

  const option = {
    title: {
      text: `${props.symbol} KLine Chart`,
      left: 'center'
    },
    tooltip: {
      trigger: 'axis',
      axisPointer: {
        type: 'cross'
      }
    },
    grid: [
      {
        left: '3%',
        right: '4%',
        height: '60%'
      },
      {
        left: '3%',
        right: '4%',
        top: '70%',
        height: '20%'
      }
    ],
    xAxis: [
      {
        type: 'category',
        data: props.data.map(item => new Date(item.time).toLocaleTimeString()),
        boundaryGap: false,
        axisLine: { onZero: false },
        splitLine: { show: false },
        min: 'dataMin',
        max: 'dataMax'
      },
      {
        type: 'category',
        gridIndex: 1,
        data: props.data.map(item => new Date(item.time).toLocaleTimeString()),
        boundaryGap: false,
        axisLine: { onZero: false },
        axisTick: { show: false },
        splitLine: { show: false },
        axisLabel: { show: false },
        min: 'dataMin',
        max: 'dataMax'
      }
    ],
    yAxis: [
      {
        scale: true,
        splitArea: { show: true }
      },
      {
        scale: true,
        gridIndex: 1,
        splitNumber: 2,
        axisLabel: { show: false },
        axisLine: { show: false },
        axisTick: { show: false },
        splitLine: { show: false }
      }
    ],
    dataZoom: [
      {
        type: 'inside',
        xAxisIndex: [0, 1],
        start: 50,
        end: 100
      },
      {
        show: true,
        xAxisIndex: [0, 1],
        type: 'slider',
        bottom: '5%',
        start: 50,
        end: 100
      }
    ],
    series: [
      {
        name: 'KLine',
        type: 'candlestick',
        data: props.data.map(item => [
          item.open,
          item.close,
          item.low,
          item.high
        ]),
        itemStyle: {
          color: '#ef5350',
          color0: '#26a69a',
          borderColor: '#ef5350',
          borderColor0: '#26a69a'
        }
      },
      {
        name: 'Volume',
        type: 'bar',
        xAxisIndex: 1,
        yAxisIndex: 1,
        data: props.data.map(item => item.volume),
        itemStyle: {
          color: (params: any) => {
            const dataIndex = params.dataIndex;
            const currentKLine = props.data[dataIndex];
            if (!currentKLine) return '#ef5350';
            return currentKLine.close >= currentKLine.open ? '#ef5350' : '#26a69a';
          }
        }
      }
    ]
  };

  chartInstance.setOption(option);
};

// 更新图表数据
const updateChart = () => {
  if (!chartInstance) return;

  chartInstance.setOption({
    xAxis: [
      {
        data: props.data.map(item => new Date(item.time).toLocaleTimeString())
      },
      {
        data: props.data.map(item => new Date(item.time).toLocaleTimeString())
      }
    ],
    series: [
      {
        name: 'KLine',
        data: props.data.map(item => [
          item.open,
          item.close,
          item.low,
          item.high
        ])
      },
      {
        name: 'Volume',
        data: props.data.map(item => item.volume)
      }
    ]
  });
};

// 监听窗口大小变化
const handleResize = () => {
  chartInstance?.resize();
};

// 监听数据变化
watch(() => props.data, () => {
  if (chartInstance) {
    updateChart();
  } else {
    initChart();
  }
}, { deep: true });

// 生命周期钩子
onMounted(() => {
  nextTick(() => {
    initChart();
    window.addEventListener('resize', handleResize);
  });
});

onUnmounted(() => {
  chartInstance?.dispose();
  window.removeEventListener('resize', handleResize);
});
</script>

<style scoped>
.kline-chart {
  width: 100%;
  height: 600px;
}
</style>