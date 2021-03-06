using System;
using NUnit.Framework;
using System.IO;
using Illumina.InterOp.Run;
using Illumina.InterOp.Metrics;
using Illumina.InterOp.Plot;
using Illumina.InterOp.Table;
using Illumina.InterOp.Comm;
using Illumina.InterOp.Summary;
using Illumina.InterOp.RunMetrics;

namespace Illumina.InterOp.Interop.UnitTest
{
	/// <summary>
	/// Test C# Swig Wrapping
	/// </summary>
	public class CoreTests
	{
	    [Test]
	    public void ListAvailablePlotMetrics()
	    {
            run_metrics run = new run_metrics();
            metric_type_description_vector metricTypeList = new metric_type_description_vector();
	        c_csharp_plot.list_available_plot_metrics(run, metricTypeList);
	        Assert.AreEqual(0, metricTypeList.Count);
	    }
		/// <summary>
		/// Test Enum parsing
		/// </summary>
		[Test]
		public void ParseMetricTypeEnum()
		{
		    Assert.AreEqual(c_csharp_run.parse_metric_type("Intensity"), metric_type.Intensity);
		}
		/// <summary>
		/// Test Enum parsing
		/// </summary>
		[Test]
		public void ToStringMetricTypeEnum()
		{
		    Assert.AreEqual(c_csharp_run.to_string(metric_type.Intensity), "Intensity");
		}
		/// <summary>
		/// Test Enum parsing
		/// </summary>
		[Test]
		public void ToStringMetricGroupEnum()
		{
		    Assert.AreEqual(c_csharp_run.to_string(metric_group.Error), "Error");
		}

		/// <summary>
		/// Test xml_file_not_found_exception
		/// </summary>
		[Test]
		public void Test_xml_file_not_found_exception()
		{
            info run_info = new info();
            try{
                run_info.read("file/not/found");
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Run.xml_file_not_found_exception){}
		}
		/// <summary>
		/// Test xml_parse_exception
		/// </summary>
		[Test]
		public void Test_xml_parse_exception()
		{
            info run_info = new info();
            try{
                run_info.parse("<RunInfo></RunInfo");
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Run.xml_parse_exception){}
		}
		/// <summary>
		/// Test empty_xml_format_exception
		/// </summary>
		[Test]
		public void Test_empty_xml_format_exception()
		{
            info run_info = new info();
            try{
                run_info.parse("");
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Run.empty_xml_format_exception){}
		}
		/// <summary>
		/// Test missing_xml_element_exception
		/// </summary>
		[Test]
		public void Test_missing_xml_element_exception()
		{
            info run_info = new info();
            try{
                run_info.parse("<RunInfo><Run><FlowcellLayout> <TileSet> </TileSet></FlowcellLayout></Run></RunInfo>");
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Run.missing_xml_element_exception){}
		}
		/// <summary>
		/// Test invalid_tile_list_exception
		/// </summary>
		[Test]
		public void Test_invalid_tile_list_exception()
		{
            info run_info = new info();
            run_info.parse("<RunInfo><Run><FlowcellLayout LaneCount=\"2\" SurfaceCount=\"2\" SwathCount=\"4\" TileCount=\"88\" FlowcellSide=\"1\"> <TileSet TileNamingConvention=\"FourDigit\"> <Tiles> <Tile>3_2101</Tile> </Tiles> </TileSet></FlowcellLayout></Run></RunInfo>");

            try{
		        run_info.validate_tiles();
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Run.invalid_tile_list_exception){}
		}
		/// <summary>
		/// Test bad_xml_format_exception
		/// </summary>
		[Test]
		public void Test_bad_xml_format_exception()
		{
            info run_info = new info();

            try{
                run_info.parse("<RunInfo></RunInfo>");
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Run.bad_xml_format_exception){}
		}
		/// <summary>
		/// Test invalid_read_exception
		/// </summary>
		[Test]
		public void Test_invalid_read_exception()
		{
            info run_info = new info();

            try{
                run_info.read(8);
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Run.invalid_read_exception){}
		}
		/// <summary>
		/// Test invalid_tile_naming_method
		/// </summary>
		[Test]
		public void Test_invalid_tile_naming_method()
		{
            info run_info = new info();

            try{
                run_info.validate();
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Run.invalid_tile_naming_method){}
		}
		/// <summary>
		/// Test invalid_run_info_exception
		/// </summary>
		[Test]
		public void Test_invalid_run_info_exception()
		{
            info run_info = new info();

            try{
                run_info.validate(20, 9999, "Test");
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Run.invalid_run_info_exception){}
		}
		/// <summary>
		/// Test invalid_run_info_cycle_exception
		/// </summary>
		[Test]
		public void Test_invalid_run_info_cycle_exception()
		{
            info run_info = new info(new flowcell_layout(8, 2, 4, 99));

            try{
                run_info.validate_cycle(1, 1101, 3000, "Test");
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Run.invalid_run_info_cycle_exception){}
		}

		/// <summary>
		/// Test bad_format_exception
		/// </summary>
		[Test]
		public void Test_bad_format_exception()
		{
		    int[] tmp = new int[]{
                 0,3
                 ,7,0,90,4,1,0,-12,-56,15,64,-98,35,12,64,0,0,0,0,0,0,0,0,46,1,17,1,0,0,0,0,96,-41,-104,36,122,-86,-46,-120
                 ,7,0,-66,4,1,0,96,-43,14,64,-63,49,13,64,0,0,0,0,0,0,0,0,56,1,17,1,0,0,0,0,112,125,77,38,122,-86,-46,-120
                 ,7,0,66,8,1,0,74,-68,6,64,-118,-7,8,64,0,0,0,0,0,0,0,0,93,1,46,1,0,0,0,0,-47,-104,2,40,122,-86,-46,-120
            };
         	byte[] expected_binary_data = new byte[tmp.Length];
         	for(int i=0;i<expected_binary_data.Length;i++) expected_binary_data[i] = (byte)tmp[i];
         	base_extraction_metrics expected_metric_set = new base_extraction_metrics();

            try{
         	    c_csharp_comm.read_interop_from_buffer(expected_binary_data, (uint)expected_binary_data.Length, expected_metric_set);
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Comm.bad_format_exception){}
		}
		/// <summary>
		/// Test incomplete_file_exception
		/// </summary>
		[Test]
		public void Test_incomplete_file_exception()
		{
		    int[] tmp = new int[]{
                 3,38
                 ,7,0,90,4,1,0,-12,-56,15,64,-98,35,12,64,0,0,0,0,0,0,0,0,46,1,17,1,0,0,0,0,96,-41,-104,36,122,-86,-46,-120
                 ,7,0,-66,4,1,0,96,-43,14,64,-63,49,13,64,0,0,0,0,0,0,0,0,56,1,17,1,0,0,0,0,112,125,77,38,122,-86,-46,-120
                 ,7,0,66,8,1,0,74,-68,6,64,-118,-7,8,64,0,0,0,0,0,0,0,0,93,1,46,1,0,0,0,0,-47,-104,2,40,122,-86,-46,-120
            };
         	byte[] expected_binary_data = new byte[tmp.Length];
         	for(int i=0;i<expected_binary_data.Length;i++) expected_binary_data[i] = (byte)tmp[i];
         	base_extraction_metrics expected_metric_set = new base_extraction_metrics();

            try{
         	    c_csharp_comm.read_interop_from_buffer(expected_binary_data, (uint)expected_binary_data.Length, expected_metric_set);
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Comm.incomplete_file_exception){}
		}
		/// <summary>
		/// Test invalid_argument
		/// </summary>
		[Test]
		public void Test_invalid_argument()
		{
		    int[] tmp = new int[]{
                2,38
                 ,7,0,90,4,1,0,-12,-56,15,64,-98,35,12,64,0,0,0,0,0,0,0,0,46,1,17,1,0,0,0,0,96,-41,-104,36,122,-86,-46,-120
                 ,7,0,-66,4,1,0,96,-43,14,64,-63,49,13,64,0,0,0,0,0,0,0,0,56,1,17,1,0,0,0,0,112,125,77,38,122,-86,-46,-120
                 ,7,0,66,8,1,0,74,-68,6,64,-118,-7,8,64,0,0,0,0,0,0,0,0,93,1,46,1,0,0,0,0,-47,-104,2,40,122,-86,-46,-120
            };
         	byte[] expected_binary_data = new byte[tmp.Length];
         	for(int i=0;i<expected_binary_data.Length;i++) expected_binary_data[i] = (byte)tmp[i];
         	base_extraction_metrics expected_metric_set = new base_extraction_metrics();
         	c_csharp_comm.read_interop_from_buffer(expected_binary_data, (uint)expected_binary_data.Length, expected_metric_set);
            byte[] newBuffer = new byte[3];

            try{
			    c_csharp_comm.write_interop_to_buffer(expected_metric_set, newBuffer, (uint)newBuffer.Length);
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Comm.invalid_argument){}
		}
		/// <summary>
		/// Test invalid_argument
		/// </summary>
		[Test]
		public void Test_invalid_filter_option()
		{
            info run_info = new info();
            filter_options options = new filter_options(tile_naming_method.FourDigit);

            try{
                options.validate(metric_type.Intensity, run_info);
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Plot.invalid_filter_option){}
		}
		/// <summary>
		/// Test invalid_column_type
		/// </summary>
		[Test]
		public void Test_invalid_column_type()
		{
            string_vector channels = new string_vector();
            bool_vector filled = new bool_vector();
            imaging_column_vector columns = new imaging_column_vector();
            try{
                c_csharp_table.create_imaging_table_columns(channels, filled, columns);
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Table.invalid_column_type){}
		}
		/// <summary>
		/// Test invalid_parameter
		/// </summary>
		[Test]
		public void Test_invalid_parameter()
		{
            uchar_vector valid_to_load = new uchar_vector();
            valid_to_load.Add(0);
            run_metrics metrics = new run_metrics();
            try{
                metrics.read_metrics("", 3, valid_to_load, 1);
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.RunMetrics.invalid_parameter){}
		}
		/// <summary>
		/// Test invalid_metric_type for list_metrics_to_load
		/// </summary>
		[Test]
		public void Test_invalid_metric_type()
		{
            uchar_vector valid_to_load = new uchar_vector();
            try{
		        c_csharp_run_metrics.list_metrics_to_load("Unknown", valid_to_load);
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.RunMetrics.invalid_metric_type){}
		}
		/// <summary>
		/// Test invalid_metric_type for plot_flowcell_map
		/// </summary>
		[Test]
		public void Test_invalid_metric_type_for_plot_flowcell_map()
		{
            uint ALL_IDS = (uint)filter_options.UseAll.ALL_IDS;
		    filter_options options = new filter_options(tile_naming_method.FourDigit, ALL_IDS, 0, dna_bases.A, ALL_IDS, 1, 1);
		    run_metrics run = new run_metrics();
            flowcell_data data = new flowcell_data();
            try{
                c_csharp_plot.plot_flowcell_map(run, "NoMetric", options, data);
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.RunMetrics.invalid_metric_type){}
		}
		/// <summary>
		/// Test invalid_channel_exception
		/// </summary>
		[Test]
		public void Test_invalid_channel_exception()
		{
            run_metrics metrics = new run_metrics();
            try{
                metrics.finalize_after_load();
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.RunMetrics.invalid_channel_exception){}
		}
		/// <summary>
		/// Test invalid_parameter base exception
		/// </summary>
		[Test]
		public void Test_invalid_parameter_base()
		{
            uchar_vector valid_to_load = new uchar_vector();
            valid_to_load.Add(0);
            run_metrics metrics = new run_metrics();
            try{
                metrics.read_metrics("", 3, valid_to_load, 1);
                Assert.Fail( "No exception thrown!" );
            }catch(Exception){}
		}



		/// <summary>
		/// Test IndexOutOfBoundsException
		/// </summary>
		[Test]
		public void TestHeatMapIndexOutOfBoundsException()
		{
            heatmap_data heatmap = new heatmap_data();
            try{
                System.Console.WriteLine(heatmap.at(0,0));
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Metrics.index_out_of_bounds_exception){}
		}
		/// <summary>
		/// Test IndexOutOfBoundsException
		/// </summary>
		[Test]
		public void TestImagingTableIndexOutOfBoundsException()
		{
            imaging_table table = new imaging_table();
            try{
                System.Console.WriteLine(table.at(0u,(column_id)0));
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Metrics.index_out_of_bounds_exception){}
		}
		/// <summary>
		/// Test IndexOutOfBoundsException
		/// </summary>
		[Test]
		public void TestIndexLaneSummaryIndexOutOfBoundsException()
		{
            index_lane_summary summary = new index_lane_summary();
            try{
                System.Console.WriteLine(summary.at(0u));
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Metrics.index_out_of_bounds_exception){}
		}
		/// <summary>
		/// Test IndexOutOfBoundsException
		/// </summary>
		[Test]
		public void TestIndexFlowcellSummaryIndexOutOfBoundsException()
		{
            index_flowcell_summary summary = new index_flowcell_summary();
            try{
                System.Console.WriteLine(summary.at(0u));
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Metrics.index_out_of_bounds_exception){}
		}
		/// <summary>
		/// Test IndexOutOfBoundsException
		/// </summary>
		[Test]
		public void TestRunSummaryIndexOutOfBoundsException()
		{
            run_summary summary = new run_summary();
            try{
                System.Console.WriteLine(summary.at(0u));
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Metrics.index_out_of_bounds_exception){}
		}
		/// <summary>
		/// Test IndexOutOfBoundsException
		/// </summary>
		[Test]
		public void TestReadSummaryIndexOutOfBoundsException()
		{
            read_summary summary = new read_summary();
            try{
                System.Console.WriteLine(summary.at(0u));
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Metrics.index_out_of_bounds_exception){}
		}
		/// <summary>
		/// Test IndexOutOfBoundsException
		/// </summary>
		[Test]
		public void TestLaneSummaryIndexOutOfBoundsException()
		{
            lane_summary summary = new lane_summary();
            try{
                System.Console.WriteLine(summary.at(0u));
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Metrics.index_out_of_bounds_exception){}
		}
		/// <summary>
		/// Test IndexOutOfBoundsException
		/// </summary>
		[Test]
		public void TestTileMetricsIndexOutOfBoundsException()
		{
            base_tile_metrics summary = new base_tile_metrics();
            try{
                System.Console.WriteLine(summary.at(0u));
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Metrics.index_out_of_bounds_exception){}
		}
		/// <summary>
		/// Test IndexOutOfBoundsException
		/// </summary>
		[Test]
		public void TestBarVectorIndexOutOfBoundsException()
		{
            bar_vector points = new bar_vector();
            try{
                System.Console.WriteLine(points[0]);
                Assert.Fail( "No exception thrown!" );
            }catch(System.ArgumentOutOfRangeException){}
		}
		/// <summary>
		/// Test IndexOutOfBoundsException
		/// </summary>
		[Test]
		public void TestCandleStickVectorIndexOutOfBoundsException()
		{
            candle_stick_vector points = new candle_stick_vector();
            try{
                System.Console.WriteLine(points[0]);
                Assert.Fail( "No exception thrown!" );
            }catch(System.ArgumentOutOfRangeException){}
		}
		/// <summary>
		/// Test IndexOutOfBoundsException
		/// </summary>
		[Test]
		public void TestCandleStickCollectionIndexOutOfBoundsException()
		{
            candle_stick_collection points = new candle_stick_collection();

            try{
                System.Console.WriteLine(points.at(0));
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Metrics.index_out_of_bounds_exception){}
		}
		/// <summary>
		/// Test IndexOutOfBoundsException
		/// </summary>
		[Test]
		public void TestBarCollectionIndexOutOfBoundsException()
		{
            bar_collection points = new bar_collection();

            try{
                System.Console.WriteLine(points.at(0));
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Metrics.index_out_of_bounds_exception){}
		}
		/// <summary>
		/// Test IndexOutOfBoundsException
		/// </summary>
		[Test]
		public void TestCandleStickSeriesIndexOutOfBoundsException()
		{
            candle_stick_series points = new candle_stick_series();

            try{
                System.Console.WriteLine(points.at(0));
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Metrics.index_out_of_bounds_exception){}
		}
		/// <summary>
		/// Test IndexOutOfBoundsException
		/// </summary>
		[Test]
		public void TestBarSeriesIndexOutOfBoundsException()
		{
            bar_series points = new bar_series();

            try{
                System.Console.WriteLine(points.at(0));
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Metrics.index_out_of_bounds_exception){}
		}
		/// <summary>
		/// Test IndexOutOfBoundsException
		/// </summary>
		[Test]
		public void TestCandleStickDataIndexOutOfBoundsException()
		{
            candle_stick_plot_data points = new candle_stick_plot_data();

            try{
                System.Console.WriteLine(points.at(0));
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Metrics.index_out_of_bounds_exception){}
		}
		/// <summary>
		/// Test IndexOutOfBoundsException
		/// </summary>
		[Test]
		public void TestBarDataIndexOutOfBoundsException()
		{
            bar_plot_data points = new bar_plot_data();

            try{
                System.Console.WriteLine(points.at(0));
                Assert.Fail( "No exception thrown!" );
            }catch(Illumina.InterOp.Metrics.index_out_of_bounds_exception){}
		}
		[Test]
		public void TestToFeatureForMetricType()
		{
            metric_type metricType = metric_type.Intensity;
            Assert.AreEqual((int)c_csharp_run_metrics.to_feature(metricType), 18);
		}
		[Test]
		public void TestDispose()
		{
            run_metrics tmp = new run_metrics();
            using(base_extraction_metrics set = tmp.extraction_metric_set())
            {
                ushort_vector tmp1 = new ushort_vector();
                float_vector tmp2 = new float_vector();
                set.insert(new extraction_metric(1,1,1, tmp1, tmp2));
            }
            using(base_extraction_metrics set = tmp.extraction_metric_set())
            {
                Assert.AreEqual((int)set.size(), 1);
            }
            using(base_extraction_metrics set = tmp.extraction_metric_set())
            {
                Assert.AreEqual((int)set.at(0).lane(), 1);
            }
		}
		[Test]
		public void TestReadFromBinaryBuffer()
		{
			int[] tmp = new int[]{2, 48, 1, 0, 80, 4, 25, 0, 39, 4, 189, 4, 198, 3, 192, 3, 71, 4, 230, 15, 234, 15, 189, 15, 132,
				15, 0, 0, 0, 0, 65, 168, 10, 0, 93, 93, 8, 0, 104, 95, 8, 0, 238, 221, 9, 0, 91, 34, 63, 65, 1, 0,
				80, 4, 1, 0, 15, 5, 22, 6, 127, 4, 134, 4, 13, 5, 149, 19, 119, 19, 51, 19, 68, 19, 186, 42, 0, 0,
				221, 49, 11, 0, 101, 53, 8, 0, 168, 76, 8, 0, 80, 100, 9, 0, 5, 226, 84, 65, 1, 0, 81, 4, 25, 0, 1,
				4, 147, 4, 164, 3, 144, 3, 45, 4, 91, 15, 91, 15, 83, 15, 38, 15, 0, 0, 0, 0, 171, 201, 10, 0, 153,
				125, 8, 0, 35, 124, 8, 0, 135, 250, 9, 0, 130, 213, 59, 65
			};
	        byte[] expected_binary_data = new byte[tmp.Length];
	        for(int i=0;i<expected_binary_data.Length;i++) expected_binary_data[i] = (byte)tmp[i];
	        run_metrics metrics = new run_metrics();
	        metrics.read_metrics_from_buffer(metric_group.CorrectedInt, expected_binary_data, (uint)expected_binary_data.Length);
	        Assert.AreEqual(metrics.corrected_intensity_metric_set().size(), 3u);
		}
		[Test]
		public void TestWriteToBinaryBuffer()
		{
		    run_metrics metrics = new run_metrics();
		    base_corrected_intensity_metrics expected_metrics = metrics.corrected_intensity_metric_set();

            expected_metrics.set_version(3);
			uint_vector calledCounts1 = new uint_vector(new uint[]{52, 1049523, 654071, 500476, 982989});
			float_vector correctedIntCalled1 = new float_vector(new float[]{245, 252, 61, 235});
			expected_metrics.insert(new corrected_intensity_metric(7, 1114, 1, correctedIntCalled1, (calledCounts1)));
			uint_vector calledCounts2 = new uint_vector(new uint[]{0, 1063708, 582243, 588028, 953132});
			float_vector correctedIntCalled2 = new float_vector(new float[]{232, 257, 68, 228});
			expected_metrics.insert(new corrected_intensity_metric(7, 1114, 2, (correctedIntCalled2), (calledCounts2)));
			uint_vector calledCounts3 = new uint_vector(new uint[]{0, 1022928, 617523, 594836, 951825});
			float_vector correctedIntCalled3 = new float_vector(new float[]{227, 268, 68, 229});
			expected_metrics.insert(new corrected_intensity_metric(7, 1114, 3, (correctedIntCalled3), (calledCounts3)));

		    byte[] expected_binary_data = new byte[metrics.calculate_buffer_size(metric_group.CorrectedInt)];
		    metrics.write_metrics_to_buffer(metric_group.CorrectedInt, expected_binary_data, (uint)expected_binary_data.Length);
		}

	}
}
