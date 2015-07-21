var villageview = angular.module('VillageView', []);

villageview.controller('villageviewCtrl', ['$scope', '$window', '$location', '$modal', '$rootScope', 'Regions', 'GPRegions', 'Workers', 'Works', 'Musters', 'Vstats',
    function ($scope, $window, $location, $modal, $rootScope, Regions, GPRegions, Workers, Works, Musters, Vstats) {

        //*****************[ Region Handelers ]*******************//
        $scope.years = ['2014-2015', '2013-2014', '2012-2013'];
        $scope.selectedYear = $scope.years[0];


        Regions.fetch($scope.selectedYear).then(function (data) {
            $scope.regions = data;
        });


        $scope.$watch('selectedState', function () {
            if ($scope.regions) fetchDistricts($scope.selectedState)
        });
        $scope.$watch('selectedDistrict', function () {
            if ($scope.regions) fetchBlocks($scope.selectedState, $scope.selectedDistrict);
        });
        $scope.$watch('selectedBlock', function () {
            if ($scope.regions) fetchGPs(leftPad($scope.selectedState, 2) + leftPad($scope.selectedDistrict, 2) + leftPad($scope.selectedBlock, 3));
        });
        $scope.$watch('selectedGP', function () {
            if ($scope.selectedGP) {
                Vstats.fetch($scope.selectedGP).then(function (response) {
                    $scope.vstats = response[0];
                });
            }

            if ($scope.regions) {
                $scope.unloadCol(1);
            }

        });


        function fetchDistricts(selectedState) {

            $scope.districts = [];
            $scope.selectedDistrict = null;
            $scope.districts = $scope.regions[1][selectedState];
        };

        function fetchBlocks(selectedState, selectedDistrict) {
            $scope.blocks = [];
            $scope.selectedBlock = null;
            if (selectedState) $scope.blocks = $scope.regions[2][selectedState][selectedDistrict];
        };

        function fetchGPs(selectedBlock) {
            $scope.gps = [];
            $scope.selectedGP = null;
            if ($scope.selectedBlock && $scope.selectedYear) {
                GPRegions.fetch(selectedBlock, $scope.selectedYear).then(function (response) {
                    $scope.gps = response[0];
                });
            };
        };

        function buildCode() {
            // Only State
            if ($scope.selectedState && !$scope.selectedDistrict && !$scope.selectedBlock && !$scope.selectedGP) {
                return {
                    code_type: 'state',
                    code: leftPad($scope.selectedState, 2),
                    type: 's'
                };
            };
            // State + District
            if ($scope.selectedState && $scope.selectedDistrict && !$scope.selectedBlock && !$scope.selectedGP) {
                return {
                    code_type: 'district',
                    code: leftPad($scope.selectedState, 2) + leftPad($scope.selectedDistrict, 2),
                    type: 'd'
                };
            };
            // State + District + Blocks
            if ($scope.selectedState && $scope.selectedDistrict && $scope.selectedBlock && !$scope.selectedGP) {
                return {
                    code_type: 'block',
                    code: leftPad($scope.selectedState, 2) + leftPad($scope.selectedDistrict, 2) + leftPad($scope.selectedBlock, 3),
                    type: 'b'
                };
            };
            // State + District + Blocks + GP
            if ($scope.selectedState && $scope.selectedDistrict && $scope.selectedBlock && $scope.selectedGP) {
                return {
                    code_type: 'panchayat',
                    code: leftPad($scope.selectedGP, 10),
                    type: 'p'
                };
            };
        };

        // Generic Number Convertor Function: leftPad(1, 2) ---> 01

        function leftPad(number, targetLength) {
            var output = number + '';
            while (output.length < targetLength) {
                output = '0' + output;
            }
            return output;
        };

        $scope.getKeyByValue = function (obje, value) {
            for (var prop in obje) {
                if (obje.hasOwnProperty(prop)) {
                    if (obje[prop] == value) {
                        return prop;
                    }
                }
            }
        };

        $scope.parseDate = function (jsonDate) {
            if (jsonDate == '')
                newdate = '';
            else

                newdate = new Date(jsonDate.replace(/(\d{2})\/(\d{2})\/(\d{4})/, "$2/$1/$3"));
            return newdate;
        }


        // Village View    --- Initialization
        $scope.isStat = false;
        $scope.switchview = function () {
            $scope.isStat = !$scope.isStat;
        };


        var columnList = ['work', 'worker', 'muster'];
        $scope.column = {
            1: {
                'status': false
            },
            2: {
                'status': false
            },
            3: {
                'status': false
            }
        };
        $scope.activeRow = {
            work: {},
            worker: {},
            muster: {}
        };

        // Functions
        function fetchColumnResults(colName, params) {

            var params;
            if (colName == 'work') {

                params = 'panchayat_code=' + leftPad($scope.selectedGP, 10) +
                        '&mustrolid=' +
                        '&workerId=';

                if ($scope.activeRow.worker.worker_code) {
                    params = 'panchayat_code=' + leftPad($scope.selectedGP, 10) +
                        '&mustrolid=' +
                         '&workerId=' + $scope.activeRow.worker.worker_code;
                }
                if ($scope.activeRow.muster.msr_no) {
                    params = 'panchayat_code=' + leftPad($scope.selectedGP, 10) +
                        '&mustrolid=' + $scope.activeRow.muster.msr_no +
                         '&workerId=';
                }
                if ($scope.activeRow.muster.mustrolid && $scope.activeRow.worker.worker_code) {
                    params = 'panchayat_code=' + leftPad($scope.selectedGP, 10) +
                        '&mustrolid=' + $scope.activeRow.muster.msr_no +
                        '&workerId=' + $scope.activeRow.worker.worker_code;
                };

                Works.fetch(params).then(function (response) {
                    $scope.works = response;
                });
            };
            if (colName == 'muster') {

                params = 'panchayat_code=' + leftPad($scope.selectedGP, 10) +
                '&Workid=' +
                '&workerId=';

                if ($scope.activeRow.work.work_code) {
                    params = 'panchayat_code=' + leftPad($scope.selectedGP, 10) +
                        '&Workid=' + $scope.activeRow.work.work_code +
                        '&workerId=';

                }
                if ($scope.activeRow.worker.worker_code) {
                    params = 'panchayat_code=' + leftPad($scope.selectedGP, 10) +
                        '&Workid=' +
                        '&workerId=' + $scope.activeRow.worker.worker_code;

                }
                if ($scope.activeRow.work.work_code && $scope.activeRow.worker.worker_code) {
                    params = 'panchayat_code=' + leftPad($scope.selectedGP, 10) +
                        '&Workid=' + $scope.activeRow.work.work_code +
                        '&workerId=' + $scope.activeRow.worker.worker_code;
                }


                Musters.fetch(params).then(function (response) {
                    $scope.musters = response;
                });
            };
            if (colName == 'worker') {

                params = 'panchayat_code=' + leftPad($scope.selectedGP, 10) +
                    '&mustrolid=' +
                    '&Workid=';

                if ($scope.activeRow.muster.msr_no) {
                    params = 'panchayat_code=' + leftPad($scope.selectedGP, 10) +
                        '&mustrolid=' + $scope.activeRow.muster.msr_no +
                         '&Workid=';
                }
                if ($scope.activeRow.work.work_code) {
                    params = 'panchayat_code=' + leftPad($scope.selectedGP, 10) +
                        '&mustrolid=' +
                         '&Workid=' + $scope.activeRow.work.work_code;
                }
                if ($scope.activeRow.muster.msr_no && $scope.activeRow.work.work_code) {
                    params = 'panchayat_code=' + leftPad($scope.selectedGP, 10) +
                        '&mustrolid=' + $scope.activeRow.muster.msr_no +
                        '&Workid=' + $scope.activeRow.work.work_code;
                }
                Workers.fetch(params).then(function (response) {
                    $scope.workers = response;
                });
            };
        }

        function whichColumn(colName) {
            var colNo;
            columnList.forEach(function (value, index) {
                if ($scope.column[index + 1].name == colName) {
                    colNo = index + 1
                }
            });
            return colNo
        }

        // Load Column 
        $scope.loadColumn = function (colName, colNo) {
            $scope.column[colNo].name = colName;
            $scope.column[colNo].status = true;

            if (colNo == 2) {
                columnList.forEach(function (value) {
                    if (value != $scope.column['1'].name && value != $scope.column['2'].name) {
                        $scope.column[3].name = value;
                        $scope.column[3].status = true;
                    }
                });
            }

            if (colNo == 1) fetchColumnResults(colName);

        };

        // Load Row
        $scope.loadRow = function (parentColName, entity) {
            parentColNo = whichColumn(parentColName);

            if (parentColNo == 3) {
                return 0;
            } else {
                childColName = $scope.column[parentColNo + 1].name;
            }

            $scope.activeRow[parentColName] = entity;
            if (parentColNo == 1) {
                $scope[$scope.column['3'].name + 's'] = [];
            }
            //Prepare params here or in fetchColumnResults
            fetchColumnResults(childColName);

        }

        // Unload Column
        $scope.unloadCol = function (colNo) {
            if (colNo === 1) {
                $scope.column = {
                    1: {
                        'status': false,
                        'name': null
                    },
                    2: {
                        'status': false,
                        'name': null
                    },
                    3: {
                        'status': false,
                        'name': null
                    }
                };
                $scope.activeRow = {
                    work: {},
                    worker: {},
                    muster: {}
                };
                $scope.workers = [];
                $scope.works = [];
                $scope.musters = [];
            } else if (colNo === 2) {
                $scope.activeRow[$scope.column['2'].name] = {};
                $scope[$scope.column['2'].name + 's'] = [];
                $scope[$scope.column['3'].name + 's'] = [];
                $scope.column['2'].status = false;
                $scope.column['2'].name = null;
                $scope.column['3'].status = false;
                $scope.column['3'].name = null;


            } else if (colNo === 3) {
                $scope.column['3'].status = false;
                $scope.column['3'].name = null;

            }

        };


        $scope.open = function (item, itemType) {
            var modalInstance = $modal.open({
                templateUrl: 'myModalContent.html',
                controller: 'ModalInstanceCtrl',
                size: 'lg',
                resolve: {
                    detailItem: function () {
                        item.itemType = itemType;
                        item.activeRow = $scope.activeRow;
                        item.panchayat_code = leftPad($scope.selectedGP, 10);
                        return item;
                    }
                }
            });
        };

    }
]);



    villageview.controller('ModalInstanceCtrl', function ($scope, $modalInstance, detailItem, Works, Workers) {
        $scope.detailItem = detailItem;
        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        // Dictionary 
        $scope.dict= {            
            work_name: 'Work Name',
            work_code: 'Work Code',
            contingency: 'Contingency',
            start_date_for_work: 'Work Start Date',
            completion_date_for_work: 'Work Completion Date',
            Sanc_fin_dt: 'Sanction Date',
            Sanc_fin_no: 'Sanction No.',
            estimated_duration: 'Estimated Duration',
            estimated_cost: 'Estimated Cost',
            scope_start_status: 'Start Status',
            scope_end_status: 'End Status',
            five_year_plan: 'Whether included in Five Year Plan',
            approved_annual_plan: 'Whether Annual Plan approved',
            photo_before: 'Photo before work started',
            photo_during: 'Photo during work',
            photo_after: 'Photo after work completed',
            workstatus: 'Status of Work',
            work_cat_code: 'Category of Work',
            work_fin_yr: 'Financial Year of work creation',
            total_exp_for_work: 'Total Expenditure occurred',
            unskilled_exp_for_muster: 'Unskilled Expenditure (Muster)',
            unskilled_exp_for_worker: 'Unskilled Expenditure (Worker)',
            unskilled_exp_for_work: 'Unskilled Expenditure (Work)',
            unskilled_persondays_work: 'Unskilled Persondays (Work)',
            unskilled_persondays_muster: 'Unskilled Persondays (Muster)',
            unskilled_workers_muster: 'Unskilled Workers (Muster)',
            semiskilled_exp_muster: 'Semiskilled Expenditure (Muster)',
            skilled_exp_muster: 'Skilled Expenditure (Muster)',
            semiskilled_exp_worker: 'Semiskilled Expenditure (Worker)',
            skilled_exp_worker: 'Skilled Expenditure (Worker)',
            semiskilled_exp_work: 'Semiskilled Expenditure (Worker)',
            Skilled_exp_work: 'Skilled Expenditure (Work)',
            unskilled_persondays_worker: 'Unskilled Persondays (Worker)',
            Total_persondays_worker: 'Total Persondays (Worker)',
            unskilled_workers_Work: 'Unskilled Workers (Work)',
            Total_persondays_muster: 'Total Persondays (Muster)',
            Total_worker_muster: 'Total Workers (Muster)',
            Total_Persondays_work: 'Total Persondays (Work)',
            Total_Worker_work: 'Total Workers (Work)',
            semiskilled_persondays_muster: 'Semiskilled Persondays (Muster)',
            semiskilled_workers_muster: 'Semiskilled Workers (Muster)',
            skilled_persondays_muster: 'Skilled Persondays (Muster)',
            skilled_workers_muster: 'Skilled Workers (Muster)',
            material_exp_work: 'Material Expenditure (Work)',
            semiskilled_persondays_worker: 'Semiskilled Persondays (Worker)',
            skilled_persondays_worker: 'Skilled Persondays (Worker)',
            semiskilled_persondays_work: 'Semiskilled Persondays (Work)',
            semiskilled_workers_work: 'Semiskilled Workers (Work)',
            skilled_persondays_work: 'Skilled Persondays (Work)',
            skilled_Workers_work: 'Skilled Workers (Work)',
            msr_no: 'MSR No.',
            payment_date: 'Date of Payment',
            panchayat_code: 'Gram Panchayat Code',
            total_dues: 'Total Dues',
            tot_persondays: 'Total Persondays',
            work_approval_date: 'Date of work approval',
            work_code: 'Work Code',
            work_end_date: 'Work End Date',
            work_name: 'Work Name',
            work_start_date: 'Work Start Date',            
            address: 'Address',
            age_at_reg: 'Age at registration',
            bpl_status: 'Whether BPL',
            current_account_no: 'Account No',
            current_bank_po: 'Account Type (Bank / Post Office)',
            gender: 'Gender',
            hoh_name: 'Name of the Head of family',
            job_card_number: 'Job Card No.',
            person_id: 'Person ID',
            reg_date: 'Date of Registration',
            sc_st_category: 'Whether SC / ST',
            Village_name: 'Village Name',
            worker_code: 'Worker Code',
            worker_name: 'Workers Name',
            ac_credited_date: 'Date of credit in account',
            average_daily_wage: 'Average Daily Wage',
            bank_po_name: 'Bank / Postoffice Name',
            no_days_work_for_muster: 'No. Days work for Muster',
            total_to_be_paid_for_muster: 'Total Paid for Muster',
            pending_payment_for_muster: 'Pending Payment for Muster',
            po_address_branch_code: 'PO/Bank Branch Address',
            po_code_branch_name: 'PO/Bank Branch Name',
            status: 'Status',
            Present: 'Present',
            tool_payments: 'Total Payments',
            total_cash_payments: 'Total Cash Payments',
            travel_food_expenses: 'Travel/Food Expenses',
            wagelist_no: 'Wagelist No.',
            days_worked: 'Days Worked',
            pending_payment: 'Pending Payments',
            total_to_be_paid: 'Total Amount Paid',
            gp_name: 'Gram Panchayat Name',
            pct_f_persondays: 'Female Persondays %',
            pct_sc_persondays: 'SC Persondays %',
            pct_st_persondays: 'ST Persondays %',
            total_persondays: 'Total Persondays ',
            total_workers: 'Total Workers',
            total_completed: 'Number of works completed',
            total_inprogress: 'Number of works in progress',
            total_started: 'Number of works started',
            total_works: 'Total Workers',
            total_wages: 'Total Wages'
        };



        function extend(a, b) {
            for (var key in b)
                if (b.hasOwnProperty(key))
                    a[key] = b[key];
            return a;
        }
        if (detailItem.itemType == 'work') {
            var mustid = detailItem.activeRow.muster.msr_no || '';
            var workerid = detailItem.activeRow.worker.worker_code || ''
            var params = 'work_code=' + detailItem.work_code +
                        '&panchayat_code=' + detailItem.panchayat_code +
                        '&pfin_yr=' + detailItem.work_fin_yr +
                        '&work_start=' + detailItem.start_date_for_work +
                        '&completion_date=' + detailItem.completion_date_for_work +
                        '&mustrolid=' + mustid+
                         '&workerId=' + workerid ;
            Works.fetchDetail(params).then(function (response) {
                delete detailItem.activeRow;
                $scope.detailItem = extend(detailItem, response);
                
            });
        }
        if (detailItem.itemType == 'worker') {
            var workcode=detailItem.activeRow.work.work_code || '';
            var mustrolid = detailItem.activeRow.muster.msr_no || '';
            var params = 'Worker_ID=' + detailItem.worker_code +
                         '&mustrolid=' + mustrolid +
                         '&Workid=' + workcode +
                         '&panchayat_code=' + detailItem.panchayat_code;
            Workers.fetchDetail(params).then(function (response) {
                delete detailItem.activeRow;
                $scope.detailItem = extend(detailItem, response);
                
            });
        }
        delete detailItem.activeRow;

    });