/** Unit tests for the collapsed q-score metrics
 *
 *
 *  @file
 *  @date 8/25/2015
 *  @version 1.0
 *  @copyright GNU Public License.
 */

#include <gtest/gtest.h>
#include "interop/logic/metric/q_metric.h"
#include "inc/q_collapsed_metrics_test.h"
#include "inc/q_metrics_test.h"

using namespace illumina::interop::model::metrics;
using namespace illumina::interop::model::metric_base;
using namespace illumina::interop::io;
using namespace illumina::interop;
using namespace illumina::interop::unittest;


typedef ::testing::Types<
        hardcoded_fixture<q_collapsed_v2>,
        write_read_fixture<q_collapsed_v2>,
        hardcoded_fixture<q_collapsed_v6>,
        write_read_fixture<q_collapsed_v6>
> Formats;
TYPED_TEST_CASE(q_collapsed_metrics_test, Formats);

/**
 * @class illumina::interop::model::metrics::q_collapsed_metric
 * @test Confirm version 2 of the metric can be written to and read from a stream
 * @test Confirm version 2 of the metric matches known binary file
 * @test Confirm version 6 of the metric can be written to and read from a stream
 * @test Confirm version 6 of the metric matches known binary file
 */
TYPED_TEST(q_collapsed_metrics_test, test_read_write)
{
    EXPECT_EQ(TypeParam::actual_metric_set.version(), TypeParam::expected_metric_set.version());
    EXPECT_EQ(TypeParam::actual_metric_set.size(), TypeParam::expected_metric_set.size());
    EXPECT_EQ(TypeParam::actual_metric_set.max_cycle(), TypeParam::expected_metric_set.max_cycle());

    for(typename TypeParam::const_iterator itExpected=TypeParam::expected_metric_set.begin(), itActual = TypeParam::actual_metric_set.begin();
        itExpected != TypeParam::expected_metric_set.end() && itActual != TypeParam::actual_metric_set.end();
        itExpected++,itActual++)
    {
        EXPECT_EQ(itExpected->lane(), itActual->lane());
        EXPECT_EQ(itExpected->tile(), itActual->tile());
        EXPECT_EQ(itExpected->cycle(), itActual->cycle());
    }
}


// Test if we can parse collapsed q-metrics
TEST(q_collapsed_metrics_test, test_convert_write_read)
{
    metric_set<q_metric> metrics;
    std::istringstream fin(q_v4::binary_data());
    illumina::interop::io::read_metrics(fin, metrics);


    metric_set<q_collapsed_metric> expected_metric_set;
    logic::metric::create_collapse_q_metrics(metrics, expected_metric_set);

    std::ostringstream fout;
    illumina::interop::io::write_metrics(fout, expected_metric_set);

    metric_set<q_collapsed_metric> actual_metric_set;
    std::istringstream iss(fout.str());
    illumina::interop::io::read_metrics(iss, actual_metric_set);


    EXPECT_EQ(actual_metric_set.version(), expected_metric_set.version());
    EXPECT_EQ(actual_metric_set.max_cycle(), expected_metric_set.max_cycle());
    ASSERT_EQ(actual_metric_set.size(), expected_metric_set.size());
    for(typename metric_set<q_collapsed_metric>::const_iterator itExpected=expected_metric_set.begin(),
                itActual = actual_metric_set.begin();
        itExpected != expected_metric_set.end();
        itExpected++,itActual++)
    {
        EXPECT_EQ(itExpected->lane(), itActual->lane());
        EXPECT_EQ(itExpected->tile(), itActual->tile());
        EXPECT_EQ(itExpected->cycle(), itActual->cycle());
        EXPECT_EQ(itExpected->q20(), itActual->q20());
        EXPECT_EQ(itExpected->q30(), itActual->q30());
        EXPECT_EQ(itExpected->median_qscore(), itActual->median_qscore());
    }
}

#define FIXTURE q_collapsed_metrics_test
/**
 * @class illumina::interop::model::metrics::q_collapsed_metric
 * @test Confirm binary write matches expected binary data
 * @test Confirm bad_format_exception is thrown when version is unsupported
 * @test Confirm incomplete_file_exception is thrown for a small partial record
 * @test Confirm incomplete_file_exception is thrown for a mostly complete file
 * @test Confirm bad_format_exception is thrown when record size is incorrect
 * @test Confirm file_not_found_exception is thrown when a file is not found
 * @test Confirm reading from good data does not throw an exception
 */
#include "inc/stream_tests.hpp"


