using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroMQ.Subscribers
{
    public class ApiBinanceFuturesUsdSubscriber
    {
        private readonly SubscriberSocket _footprintSubscriber;
        private readonly SubscriberSocket _heatmapSubscriber;
        private readonly SubscriberSocket _candleSubscriber;

        public ApiBinanceFuturesUsdSubscriber(BinanceZeroMQProperties options)
        {
            _candleSubscriber = SubPubFactory.NewSubscriber(1000);
            _heatmapSubscriber = SubPubFactory.NewSubscriber(1000);
            _footprintSubscriber = SubPubFactory.NewSubscriber(1000);
            string address = $"tcp://{options.PublisherIPAddress}:";

            // candle
            _candleSubscriber.Connect(address + options.BinanceFuturesUsdCandleApiPort);
            _candleSubscriber.SubscribeToAnyTopic();

            // footprint
            _footprintSubscriber.Connect(address + options.BinanceFuturesUsdFootprintApiPort);
            _footprintSubscriber.SubscribeToAnyTopic();

            // heatmap
            _heatmapSubscriber.Connect(address + options.BinanceFuturesUsdHeatmapApiPort);
            _heatmapSubscriber.SubscribeToAnyTopic();
        }

        /// <summary>
        /// waits till recieved any bytes from ServerApplication
        /// </summary>
        /// <returns>the candle pushed from ServerApplication</returns>
        public OpenCandle GetCandle()
        {
            byte[] json = _candleSubscriber.ReceiveFrameBytes();
            return Utf8Json.JsonSerializer.Deserialize<OpenCandle>(json);
        }

        /// <summary>
        /// waits till recieved any bytes from ServerApplication
        /// </summary>
        /// <returns>the footprint pushed from ServerApplication</returns>
        public OpenFootprint GetFootprint()
        {
            byte[] json = _footprintSubscriber.ReceiveFrameBytes();
            return Utf8Json.JsonSerializer.Deserialize<OpenFootprint>(json);
        }

        /// <summary>
        /// waits till recieved any bytes from ServerApplication
        /// </summary>
        /// <returns>the heatmap pushed from ServerApplication</returns>
        public OpenHeatmap GetHeatmap()
        {
            byte[] json = _heatmapSubscriber.ReceiveFrameBytes();
            return Utf8Json.JsonSerializer.Deserialize<OpenHeatmap>(json);
        }
    }
}
