<?php
 ob_get_contents();
 ob_end_clean();


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

if(isset($_SESSION["ID"])){

        $IDzalogowanego = $_SESSION["ID"];

        $res = mysqli_query($polaczenie, "SELECT rola, log FROM users WHERE ID=$IDzalogowanego");
        $row=mysqli_fetch_assoc($res);

        if(isset($row["rola"])){

            if($row["rola"] == "admin" || $row["rola"] == "admin2"){





    function PokazUserow($conn){

        $result = mysqli_query($conn, "SELECT ID, rola, log FROM users");

        if(mysqli_num_rows($result) > 0){

            while($row = mysqli_fetch_assoc($result)){

                $userID = $row["ID"];
                $login = $row["log"];
                $rola = $row["rola"];

                if($rola == "admin" && $_SESSION["ID"] == $userID){
                    echo "<option disabled value='$userID'>- $login   (jeśli chcesz usunąć swoje konto wybierz następnego admina)</option>";
                }elseif($rola == "admin"){
                    echo "<option disabled value='$userID'>- $login   (Nie możesz usunąć Głównego Admina)</option>";
                }else{
                    echo "<option value='$userID'>- $login</option>";
                }
                

            }


        }

        mysqli_free_result($result);
    }

    if(isset($_POST["usun"]) && isset($_POST["users"])){

        if($polaczenie){

            $IDusersToDel = $_POST["users"];

            foreach($IDusersToDel as $IDuserToDel){
                mysqli_query($polaczenie, "DELETE FROM users WHERE ID = $IDuserToDel");

                mysqli_query($polaczenie, "DELETE FROM comments WHERE userID = $IDuserToDel");
            }

            
        }


    }

    if(isset($_POST["oddaj"]) && isset($_POST["users"])){

        if($polaczenie){

            foreach($_POST["users"] as $IDusersToPromote){

                mysqli_query($polaczenie, "UPDATE users SET rola = 'admin' WHERE ID = $IDusersToPromote");
                break;
            }
            
            $IDbylegoAdmina = $_SESSION["ID"];
            mysqli_query($polaczenie, "UPDATE users SET rola = 'user' WHERE ID = $IDbylegoAdmina");

            header("Location: index.php");
        }

    }

    if(isset($_POST["mianuj"]) && isset($_POST["users"])){

        if($polaczenie){

            foreach($_POST["users"] as $IDusersToPromote){

                mysqli_query($polaczenie, "UPDATE users SET rola = 'admin2' WHERE ID = $IDusersToPromote");

            }
            
        }

    }

    if(isset($_POST["mianujIusun"]) && isset($_POST["users"])){
        
        if($polaczenie){

            foreach($_POST["users"] as $IDusersToPromote){

                mysqli_query($polaczenie, "UPDATE users SET rola = 'admin2' WHERE ID = $IDusersToPromote");
                break;
            }
            
            $IDbylegoAdmina = $_SESSION["ID"];
            mysqli_query($polaczenie, "DELETE FROM users WHERE ID = $IDbylegoAdmina");

            mysqli_query($polaczenie, "DELETE FROM comments WHERE userID = $IDbylegoAdmina");

            header("Location: logout.php");
        }

    }

    $nameOFlogoPathFile = "uploads/logopath.txt";
    $nameOFIcoPathFile = "uploads/Icopath.txt";
    $nameOFpageNameFile = "uploads/pagename.txt";
    $nameOFnoteFile = "uploads/pagenote.txt";
    $nameOFsloganFile = "uploads/slogan.txt";


    if(isset($_POST["NewData"])){

        $target_dir = "uploads/"; 

        if (!($_FILES["fileToUpload"]["error"] == 4 || ($_FILES["fileToUpload"]["size"] == 0 && $_FILES["fileToUpload"]["error"] == 0))){

            $uploadOk = 1;   

            $imageFileType = strtolower(pathinfo($_FILES["fileToUpload"]["name"],PATHINFO_EXTENSION)); //lowercase file extension

            $target_file = $target_dir."logo.".$imageFileType;
        
            // Check if the file is an image
            $check = getimagesize($_FILES["fileToUpload"]["tmp_name"]);
            if($check !== false) {
              //echo "<br>File is an image - " . $check["mime"] . ".";
              $uploadOk = 1;
            } else {
              echo "<script>alert('Ten plik nie jest obrazem.')</script>";
              $uploadOk = 0;
            }

        
            //Supported file formats
            if($imageFileType != "jpg" && $imageFileType != "png" && $imageFileType != "jpeg"
                && $imageFileType != "gif" && $imageFileType != "ico") {
                echo "<script>alert('Tylko pliki w formatach JPG, JPEG, ICO, PNG i GIF są dozwolone.')</script>";
                $uploadOk = 0;
            }



        
            if ($uploadOk == 1) {      // if everything is ok - upload file
            
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
                      if($txtFile){

                        fwrite($txtFile, $target_file);
                        fclose($txtFile);

                      }else{
                        echo "<script>alert('Przepraszamy, wystąpił błąd podczas zapisu do pliku zawierającego ścieżkę do loga strony.  Spróbuj ponownie.')</script>";
                      }

                    } else {
                      echo "<script>alert('Przepraszamy, wystąpił błąd. Spróbuj ponownie.')</script>";
                    }
                
                }
        }

        if (!($_FILES["IcoToUpload"]["error"] == 4 || ($_FILES["IcoToUpload"]["size"] == 0 && $_FILES["IcoToUpload"]["error"] == 0))){

            $target_dir = "uploads/";
            $uploadOKK = 1;    



            $FileType = strtolower(pathinfo($_FILES["IcoToUpload"]["name"],PATHINFO_EXTENSION)); //lowercase file extension
            $target_ico = $target_dir."icon.".$FileType;
        
            // Check if the file is an image
            $check = getimagesize($_FILES["IcoToUpload"]["tmp_name"]);
            if($check !== false) {
              //echo "<br>File is an image - " . $check["mime"] . ".";
              $uploadOKK = 1;
            } else {
              echo "<script>alert('Ten plik nie jest obrazem.')</script>";
              $uploadOKK = 0;
            }
        
            //Supported file formats
            if($FileType != "jpg" && $FileType != "png" && $FileType != "jpeg"
                && $FileType != "gif" && $FileType != "ico" ) {
                echo "<script>alert('Tylko pliki w formatach JPG, JPEG, ICO, PNG i GIF są dozwolone.')</script>";
                $uploadOKK = 0;
            }

            /*if ($_FILES["IcoToUpload"]["size"] > 41943039) {
                echo "Przepraszamy, Twój plik jest za duży.";
                $uploadOk = 0;
            }*/
        
            if ($uploadOKK == 1) {      // if everything is ok - upload file
            
                    $IconsToDel = glob('uploads/icon.*'); // get all files with name logo from uploads/
                    foreach($IconsToDel as $file){ // iterate files
                        if(is_file($file)) {
                        unlink($file); // delete file
                      }
                    }
                    clearstatcache();
                
                    if (move_uploaded_file($_FILES["IcoToUpload"]["tmp_name"], $target_ico)) {
                        //echo "The file ". htmlspecialchars( $_FILES["fileToUpload"]["name"]). " has been uploaded.";
                        
                        $IcotxtFile = fopen($nameOFIcoPathFile, "w");
                        if($IcotxtFile){
                        
                          fwrite($IcotxtFile, $target_ico);
                          fclose($IcotxtFile);
                        
                        }else{
                          echo "<script>alert('Przepraszamy, wystąpił błąd podczas zapisu do pliku zawierającego ścieżkę do ikony strony. Spróbuj ponownie.')</script>";
                        }
                    } else {
                      echo "<script>alert('Przepraszamy, wystąpił błąd. Spróbuj ponownie.')</script>";
                    }
                
            }

        }

        if(!empty($_POST["PageName"])){

            $pagename = $_POST["PageName"];

            $PageNametxtFile = fopen($nameOFpageNameFile, "w");

            if($PageNametxtFile){

                fwrite($PageNametxtFile, $pagename);
                fclose($PageNametxtFile);

              }else{
                echo "<script>alert('Przepraszamy, wystąpił błąd podczas zapisu do pliku zawierającego nazwę strony. Spróbuj ponownie.')</script>";
              }

        }

        if(!empty($_POST["PageNote"])){

            $pagenote = $_POST["PageNote"];

            $PageNotetxtFile = fopen($nameOFnoteFile, "w");

            if($PageNotetxtFile){

                fwrite($PageNotetxtFile, $pagenote);
                fclose($PageNotetxtFile);

              }else{
                echo "<script>alert('Przepraszamy, wystąpił błąd podczas zapisu do pliku zawierającego notatkę o stronie. Spróbuj ponownie.')</script>";
              }

        }

        if(!empty($_POST["Slogan"])){

            $slogan = $_POST["Slogan"];

            $SlogantxtFile = fopen($nameOFsloganFile, "w");

            if($SlogantxtFile){

                fwrite($SlogantxtFile, $slogan);
                fclose($SlogantxtFile);

            }else{
              echo "<script>alert('Przepraszamy, wystąpił błąd podczas zapisu do pliku zawierającego hasło reklamowe strony. Spróbuj ponownie.')</script>";
            }

        }

    }


    

?>

<html class="ht1">
    <head>
        <title>Panel Administratora <?php
                
                $FileTOread = @fopen($nameOFpageNameFile, "r");

                if(!$FileTOread){

                  echo "TrainaTon.pl";

                }else{

                  echo fread($FileTOread, filesize($nameOFpageNameFile));
                  fclose($FileTOread);

                }

            ?></title>
        <meta charset="utf-8">
        <link rel="stylesheet" href="StronaGlowna.css">
        <link rel="icon" href="<?php
                
                $FileTOread = @fopen($nameOFIcoPathFile, "r");

                if(!$FileTOread){

                  echo "graphics/LogoTrainaTon2.ico";

                }else{

                  echo fread($FileTOread, filesize($nameOFIcoPathFile));
                  fclose($FileTOread);

                }

            ?>
            ">
    </head>

    <body>
        <header class="pasekGorny">
            
            <a  class="LogoContainer" href="index.php">
            <img src="<?php
                
                $FileTOread = @fopen($nameOFlogoPathFile, "r");

                if(!$FileTOread){

                  echo "graphics/logoTT.png";

                }else{

                  echo fread($FileTOread, filesize($nameOFlogoPathFile));

                }
  
                ?>" alt="Logo Firmy TrainaTon" class="logo">
            </a>
            

            <nav class="nawigacja">

                <a href="index.php" class="nawigacjaPrzyciski">
                    <img src="graphics/StronaGlowna.png" alt="dom" class="ikonyPrzyciskow">Strona Główna
                </a>

                <?php /*------Jeżeli jesteś adminem to masz zakładkę panel admina---------*/


                    $IDzalogowanego = $_SESSION["ID"];

                    $result = mysqli_query($polaczenie, "SELECT rola, log FROM users WHERE ID=$IDzalogowanego");
                    $row=mysqli_fetch_assoc($result);
                    if(isset($row["rola"])){
                        
                        if($row["rola"] == "admin" || $row["rola"] == "admin2"){

                        echo '<a href="StronaAdmina.php" class="nawigacjaPrzyciski">
                                    <img src="graphics/narzędzia.png" alt="narzedzia" class="ikonyPrzyciskow">Panel Admina
                                  </a>';
                        }

                    }else{
                         header("Location: logout.php");
                    }

                    mysqli_free_result($result);


                ?>

                <a href="logowanie.php" class="nawigacjaPrzyciski">
                    <img src="graphics/logowanie.png" alt="postać" class="ikonyPrzyciskow">Konto
                </a>

            </nav>

            <div class="hasloReklamowe"><?php
                
                $FileTOread = @fopen($nameOFsloganFile, "r");

                if(!$FileTOread){

                  echo "JUŻ DZIŚ znajdź swój pociąg i skomentuj jego opóźnienie!";

                }else{

                  echo fread($FileTOread, filesize($nameOFsloganFile));
                  fclose($FileTOread);

                }

            ?>
            </div>

        </header>

        <section class="StronaDolnaAdmin">

            <section class="panelAdmina">
                    
                <div class="srodekPanelAdmina">

                    <table>

                        <tr>
                            <th class="NaglowekPanelAdmina" colspan="2">Panel Administratora</th>
                        </tr>
                        
                        <tr>

                            <td class="srodekPanelAdminaLewy">

                                <div class="wyborUserow">

                                    <h2>Lista Użytkowników Systemu:</h2>

                                    <form class="customPageForm AlignSpaceBetween" method="POST" action="StronaAdmina.php">

                                        <select class="listaUserow"  size="13" name="users[]" multiple>
                                            <?php
                                                if($polaczenie){
                                                    PokazUserow($polaczenie);
                                                }
                                            ?>
                                        </select>

                                        <section>
                                            <input class="PrzyciskiUsunUsera MniejszePrzyciskiUsun" type="submit" name="usun" value="Usuń">
                                            <input class="PrzyciskiUsunUsera MniejszePrzyciskiUsun" type="submit" name="mianuj" value="Mianuj">
                                            <input class="PrzyciskiUsunUsera MniejszePrzyciskiUsun" type="submit" name="oddaj" value="Oddaj Admina">
                                            <input class="PrzyciskiUsunUsera width100perc" type="submit" name="mianujIusun" value="Mianuj Adminem i Usuń Swoje Konto">
                                        </section>

                                    </form>

                                </div>

                            </td>

                            <td class="srodekPanelAdminaPrawy">
                                <div class="tdBlock">
                                    
                                    <!--<h2>Edytuj Stronę:</h2>-->

                                    <form class="customPageForm AlignSpaceBetween100" method="POST" action="StronaAdmina.php"  enctype="multipart/form-data">    

                                                <section class="customPageForm">
                                                    <div class="FlexRow">Zmiana Loga Strony <label class="fileUploadBox"><img src="graphics/upload.svg" class="margin_right_5">Wybierz<input type="file" name="fileToUpload"></label></div>
                                                    <div class="FlexRow">Zmiana Ikony Strony <label class="fileUploadBox"><img src="graphics/upload.svg" class="margin_right_5">Wybierz<input type="file" name="IcoToUpload"></label></div>
                                                    <div class="FlexRow">Zmiana Nazwy Strony <input class="textInput" placeholder="..." type="text" name="PageName" maxlength="25"></div>
                                                    <div class="FlexRow">Zmiana Hasła Reklamowego <input class="textInput" placeholder="..." type="text" name="Slogan" maxlength="40"></div>
                                                    <div>Zmiana Notatki o Stronie <textarea class="textareaLimit" name="PageNote"></textarea></div>
                                                </section>

                                                <input class="NewDataButton" type="submit" value="Zmień" name="NewData">
                                                
                                    </form>

                                </div>

                            </td>

                        </tr>

                    </table>

                </div>

            </section>
        </section>







    </body>
</html>

<?php
        }else{
            header("Location: index.php");
        }

    mysqli_free_result($res);

    }else{

        header("Location: logout.php");

    }

}else{

    header("Location: index.php");

}
?>