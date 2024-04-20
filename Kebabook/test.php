<?php

$db_host = "localhost";//"localhost:3307";
$db_user = "root";
$db_pass = "";
$db = "kebabook_db";

	if(isset($_GET["name"]) && 
		isset($_GET["type"]) && 
		isset($_GET["grade"]) && 
		isset($_GET["latitude"]) && 
		isset($_GET["longitude"])
	){


		$name = $_GET["name"];
		$type = $_GET["type"];
		$grade = $_GET["grade"];
		$latitude = $_GET["latitude"];
		$longitude = $_GET["longitude"];

		$comment = isset($_GET["comment"]) ? $_GET["comment"] : null;
		
	
	
		$conn = mysqli_connect($db_host, $db_user, $db_pass, $db);
	
		$query = "INSERT INTO `places_table`(`id`, `kebabook_name`, `kebabook_type`, `grade`, `comment`, `latitude`, `longitude`) VALUES ('','$name',$type,$grade,'$comment',$latitude,$longitude)";
	

		$output=[];

		if(mysqli_query($conn, $query)){

			$output[]=["value"=> "Success", "color" => "lightgreen"];

			mysqli_close($conn);

		}else{
			$output[]=["value"=> "Failure", "color" => "red"];
		}

		$json = json_encode($output);
		echo $json;


	}

	if(isset($_POST["name"]) && isset($_POST["type"]) && isset($_POST["grade"])){

		$name = $_POST["name"];
		$type = $_POST["type"];
		$grade = $_POST["grade"];

		$output = [];


		$query = "";

		if($name){

			$query = "SELECT * FROM `places_table` WHERE `kebabook_name`='$name' AND `grade`>=$grade";

		}else{

			$query = "SELECT * FROM `places_table` WHERE `grade`>=$grade";

		}

		if(intval($type) != 3){
			$query .= " AND `kebabook_type`=$type";
		}

		$conn = mysqli_connect($db_host, $db_user, $db_pass, $db);

		$result = mysqli_query($conn, $query);


		if(mysqli_num_rows($result) > 0){



			while($row = mysqli_fetch_assoc($result)){

				$id = $row["id"];
				$kebabook_name = $row["kebabook_name"];
				$kebabook_type = $row["kebabook_type"];
				$grade = $row["grade"];
				$comment = $row["comment"];
				$latitude = $row["latitude"];
				$longitude = $row["longitude"];

				$output[] = [
					"id" => $id,
					"kebabook_name" => $kebabook_name,
					"kebabook_type" => $kebabook_type,
					"grade" => $grade,
					"comment" => $comment,
					"latitude" => $latitude,
					"longitude" => $longitude
				];



			}

		}else{

			$output = ["error" => "Brak kebabowni o takich parametrach"];

		}

		$json = json_encode($output);
		echo $json;


	}


	if(isset($_POST["location_range"]) && isset($_POST["grade"]) && isset($_POST["my_latitude"]) && isset($_POST["my_longitude"])){





		$range = $_POST["location_range"];
		$grade = $_POST["grade"];
		$my_latitude = $_POST["my_latitude"];
		$my_longitude = $_POST["my_longitude"];

		$output = [];


		$query = "SELECT * FROM `places_table` WHERE `grade`>=$grade";


		$conn = mysqli_connect($db_host, $db_user, $db_pass, $db);

		$result = mysqli_query($conn, $query);


		if(mysqli_num_rows($result) > 0){

			$found = false;



			while($row = mysqli_fetch_assoc($result)){

				$latitude = $row["latitude"];
				$longitude = $row["longitude"];

				$distance = getDistance($my_latitude, $my_longitude, $latitude, $longitude);

				if($distance <= $range){

					$id = $row["id"];
					$kebabook_name = $row["kebabook_name"];
					$kebabook_type = $row["kebabook_type"];
					$grade = $row["grade"];
					$comment = $row["comment"];

					$found = true;
	
					$output[] = [
						"id" => $id,
						"kebabook_name" => $kebabook_name,
						"kebabook_type" => $kebabook_type,
						"grade" => $grade,
						"comment" => $comment,
						"latitude" => $latitude,
						"longitude" => $longitude,
						"distance" => $distance
					];

				}


			}

			if(!$found){
				$output = ["error" => "Brak kebabowni o takich parametrach"];
			}

		}else{

			$output = ["error" => "Brak kebabowni o takich parametrach"];

		}

		$json = json_encode($output);
		echo $json;


	}




	if(isset($_POST["tabela"]) && isset($_POST["my_latitude"]) && isset($_POST["my_longitude"])){

		$my_latitude = $_POST["my_latitude"];
		$my_longitude = $_POST["my_longitude"];

		$output = [];


		$query = "SELECT * FROM `places_table`";


		$conn = mysqli_connect($db_host, $db_user, $db_pass, $db);

		$result = mysqli_query($conn, $query);


		if(mysqli_num_rows($result) > 0){



			while($row = mysqli_fetch_assoc($result)){

				$latitude = $row["latitude"];
				$longitude = $row["longitude"];

				$distance = getDistance($my_latitude, $my_longitude, $latitude, $longitude);

				$id = $row["id"];
				$kebabook_name = $row["kebabook_name"];
				$kebabook_type = $row["kebabook_type"];
				$grade = $row["grade"];
				$comment = $row["comment"];


				$output[] = [
					"id" => $id,
					"kebabook_name" => $kebabook_name,
					"kebabook_type" => $kebabook_type,
					"grade" => $grade,
					"comment" => $comment,
					"distance" => $distance
				];


			}


		}else{

			$output = ["error" => "Brak kebabowni"];

		}

		$json = json_encode($output);
		echo $json;

	}



	function getDistance($latitude1, $longitude1, $latitude2, $longitude2) {  
	
		$earth_radius = 6371;
	  
		$dLat = deg2rad($latitude2 - $latitude1);  
		$dLon = deg2rad($longitude2 - $longitude1);  
	  
		$a = sin($dLat/2) * sin($dLat/2) + cos(deg2rad($latitude1)) * cos(deg2rad($latitude2)) * sin($dLon/2) * sin($dLon/2);  
		$c = 2 * asin(sqrt($a));  

		$d = $earth_radius * $c;  
	  
		return $d;  
	}


	

?>
