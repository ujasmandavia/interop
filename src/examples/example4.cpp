/** Example 4: Calculating simple statistics
 *
 *
 *  @file
 *  @date 3/1/16
 *  @version 1.0
 *  @copyright GNU Public License.
 */

// @ [Calculating simple statistics]
#include <iostream>
#include "interop/model/metric_sets/corrected_intensity_metric_set.h"
#include "interop/io/metric_file_stream.h"
#include "interop/util/statistics.h"
#include "interop/constants/enums.h"

using namespace illumina::interop::model::metrics;
using namespace illumina::interop::io;
using namespace illumina::interop::util;
using namespace illumina::interop;

int check_args(int argc);

int main(int argc, char** argv)
{
    int ret;
    if((ret = check_args(argc)) != 0) return ret;
    corrected_intensity_metrics corrected_intensity_metric_set;

    try {
        read_interop(argv[1], corrected_intensity_metric_set);
    }
    catch(const incomplete_file_exception&){}// Ignore incomplete files
    catch(const bad_format_exception& ex) // Something catastrophic happened to the file
    {
        std::cerr << "InterOp did not have the expected format: " << ex.what() << std::endl;
        return 1;
    }
    catch(const file_not_found_exception& ex)
    {
        std::cerr << "Count not find InterOp file: " << ex.what() << std::endl;
        return 1;
    }

    float avg = mean<float>(corrected_intensity_metric_set.metrics().begin(),
                            corrected_intensity_metric_set.metrics().end(),
                            op::opt1(constants::A, &corrected_intensity_metric::percentBase));
    float std = std::sqrt(variance<float>(corrected_intensity_metric_set.metrics().begin(),
                            corrected_intensity_metric_set.metrics().end(),
                            op::opt1(constants::A, &corrected_intensity_metric::percentBase)));
    std::cout << "Mean of percent base for A " << avg  << std::endl;
    std::cout << "Standard Deviation of percent base for A " << std  << std::endl;

    return 0;
}

// @ [Calculating simple statistics]

int check_args(int argc)
{
    if (argc != 2)
    {
        if (argc < 2) std::cerr << "No run folder" << std::endl;
        else std::cerr << "Too many arguments" << std::endl;
        return 1;
    }
    return 0;
}

