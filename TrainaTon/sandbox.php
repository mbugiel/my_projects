<?php
    session_start();
    $dbhost="localhost";    
    $dbuser="root";         
    $dbpass="";            
    $dbname="pkp";
try{
  $polaczenie=mysqli_connect($dbhost, $dbuser, $dbpass, $dbname);

}
catch(Exception $e){
  header("Location: index.php");
}

$nameOFlogoPathFile = "uploads/logopath.txt";

if(isset($_POST["submit"])){

    $target_dir = "uploads/";
    $uploadOk = 1;    
    
    if (!($_FILES["fileToUpload"]["error"] == 4 || ($_FILES["fileToUpload"]["size"] == 0 && $_FILES["fileToUpload"]["error"] == 0))){

        $imageFileType = strtolower(pathinfo($_FILES["fileToUpload"]["name"],PATHINFO_EXTENSION)); //lowercase file extension

        $target_file = $target_dir."logo.".$imageFileType;
    
        // Check if the file is an image
        $check = getimagesize($_FILES["fileToUpload"]["tmp_name"]);
        if($check !== false) {
          //echo "<br>File is an image - " . $check["mime"] . ".";
          $uploadOk = 1;
        } else {
          //echo "File is not an image.";
          $uploadOk = 0;
        }

      
        //Supported file formats
        if($imageFileType != "jpg" && $imageFileType != "png" && $imageFileType != "jpeg"
            && $imageFileType != "gif" ) {
            //echo "Sorry, only JPG, JPEG, PNG & GIF files are allowed.";
            $uploadOk = 0;
        }
      
        //If error occured
        if ($uploadOk == 0) {

            echo "Sorry, your file was not uploaded.";
        
        
          } else {      // if everything is ok - upload file
          
                $filesToDel = glob('uploads/logo.*'); // get all files with name logo from uploads/

                foreach($filesToDel as $file){ // iterate files

                    if(is_file($file)) {
                    unlink($file); // delete file
                  }

                }
                clearstatcache();
              
                if (move_uploaded_file($_FILES["fileToUpload"]["tmp_name"], $target_file)) {

                  //echo "The file ". htmlspecialchars( $_FILES["fileToUpload"]["name"]). " has been uploaded.";

                  $txtFile = fopen($nameOFlogoPathFile, "w");
                  if(!$txtFile){

                    $uploadOk = 0;

                  }else{

                    fwrite($txtFile, $target_file);
                    fclose($txtFile);

                  }

                } else {
                  echo "Sorry, there was an error uploading your file.";
                }
              
            }
    }
}
?>

<html>
    <head>
        <title>TrainaTon.pl</title>
        <meta charset="utf-8">
        <link rel="stylesheet" href="StronaGlowna.css">
        <link rel="icon" href="LogoTrainaTon2.ico">
    </head>

    <header class="pasekGorny">
            
            <a  class="LogoContainer" href="index.php">
                
              <img src="
                
                <?php
                
                $FileTOread = @fopen($nameOFlogoPathFile, "r");

                if(!$FileTOread){

                  echo "logoTT.png";

                }else{

                  echo fread($FileTOread, filesize($nameOFlogoPathFile));

                }
  
                ?>
                " alt="Logo Firmy TrainaTon" class="logo">
            </a>
            

            <nav class="nawigacja">

                <a href="index.php" class="nawigacjaPrzyciski">
                    <img src="StronaGlowna.png" alt="dom" class="ikonyPrzyciskow">Strona Główna
                </a>

                <?php /*------Jeżeli jesteś adminem to masz zakładkę panel admina---------*/

                    if(isset($_SESSION["ID"])){

                        $IDzalogowanego = $_SESSION["ID"];

                        $result = mysqli_query($polaczenie, "SELECT rola, log FROM users WHERE ID=$IDzalogowanego");
                        $row=mysqli_fetch_assoc($result);
                        if(isset($row["rola"])){
                        
                            if($row["rola"] == "admin" || $row["rola"] == "admin2"){

                            echo '<a href="StronaAdmina.php" class="nawigacjaPrzyciski">
                                        <img src="narzędzia.png" alt="narzedzia" class="ikonyPrzyciskow">Panel Admina
                                    </a>';
                            }
                        }else{
                            header("Location: logout.php");
                        }

                        mysqli_free_result($result);
                    }

                ?>

                <a href="logowanie.php" class="nawigacjaPrzyciski">
                    <img src="logowanie.png" alt="postać" class="ikonyPrzyciskow">Konto
                </a>

            </nav>

            <div class="hasloReklamowe">JUŻ DZIŚ znajdź swój pociąg i skomentuj jego opóźnienie!</div>

    </header>

    <body>
    <form method="POST" action="sandbox.php"  enctype="multipart/form-data">                      
                Zmiana Loga Strony <input type="file" name="fileToUpload">
                <input type="submit" value="prześlij" name="submit">
    </form>







    </body>
</html>