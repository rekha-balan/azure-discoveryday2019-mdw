create table dbo.trips_all
(
	trip_type int null,
	trip_year int null,
	trip_month varchar(100) null,
	taxi_type varchar(100) null,
	vendor_id int null,
	pickup_datetime datetime null,
	dropoff_datetime datetime null,
	passenger_count int null,
	trip_distance float null,
	rate_code_id int null,
	store_and_fwd_flag varchar(100) null,
	pickup_location_id int null,
	dropoff_location_id int null,
	pickup_longitude varchar(100) null,
	pickup_latitude varchar(100) null,
	dropoff_longitude varchar(100) null,
	dropoff_latitude varchar(100) null,
	payment_type int null,
	fare_amount float null,
	extra float null,
	mta_tax float null,
	tip_amount float null,
	tolls_amount float null,
	improvement_surcharge float null,
	ehail_fee float null,
	total_amount float null
);
go

create index ix_trip_year_trip_month
on dbo.trips_all 
(
	trip_year, trip_month
);
go

create table dbo.trips_new
(
	trip_type int null,
	trip_year int null,
	trip_month varchar(100) null,
	taxi_type varchar(100) null,
	vendor_id int null,
	pickup_datetime datetime null,
	dropoff_datetime datetime null,
	passenger_count int null,
	trip_distance float null,
	rate_code_id int null,
	store_and_fwd_flag varchar(100) null,
	pickup_location_id int null,
	dropoff_location_id int null,
	pickup_longitude varchar(100) null,
	pickup_latitude varchar(100) null,
	dropoff_longitude varchar(100) null,
	dropoff_latitude varchar(100) null,
	payment_type int null,
	fare_amount float null,
	extra float null,
	mta_tax float null,
	tip_amount float null,
	tolls_amount float null,
	improvement_surcharge float null,
	ehail_fee float null,
	total_amount float null,
	textanalytics_customer_sentiment_score float null,
	customer_comments nvarchar(max) null
);
go

create index ix_trip_year_trip_month
on dbo.trips_new 
(
	trip_year, trip_month
);
go

create view dbo.trips as
select
	trip_type,
	trip_year,
	trip_month,
	taxi_type,
	vendor_id,
	pickup_datetime,
	dropoff_datetime,
	passenger_count,
	trip_distance,
	rate_code_id,
	store_and_fwd_flag,
	pickup_location_id,
	dropoff_location_id,
	pickup_longitude,
	pickup_latitude,
	dropoff_longitude,
	dropoff_latitude,
	payment_type,
	fare_amount,
	extra,
	mta_tax,
	tip_amount,
	tolls_amount,
	improvement_surcharge,
	ehail_fee,
	total_amount,
	textanalytics_customer_sentiment_score = null,
	customer_comments = null
from
	dbo.trips_all
union all
select
	trip_type,
	trip_year,
	trip_month,
	taxi_type,
	vendor_id,
	pickup_datetime,
	dropoff_datetime,
	passenger_count,
	trip_distance,
	rate_code_id,
	store_and_fwd_flag,
	pickup_location_id,
	dropoff_location_id,
	pickup_longitude,
	pickup_latitude,
	dropoff_longitude,
	dropoff_latitude,
	payment_type,
	fare_amount,
	extra,
	mta_tax,
	tip_amount,
	tolls_amount,
	improvement_surcharge,
	ehail_fee,
	total_amount,
	textanalytics_customer_sentiment_score,
	customer_comments
from
	dbo.trips_new
;
go
