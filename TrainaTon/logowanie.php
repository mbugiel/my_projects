<?php
session_start();

$nameOFlogoPathFile = "uploads/logopath.txt";
$nameOFpageNameFile = "uploads/pagename.txt";
$nameOFIcoPathFile = "uploads/Icopath.txt";
$nameOFsloganFile = "uploads/slogan.txt";
$nameOFnoteFile = "uploads/pagenote.txt";

if(!isset($_SESSION["ID"])){

     if(isset($_POST["login"]) && isset($_POST["haslo"])){



         $dbhost="localhost";    
         $dbuser="root";         
         $dbpass="";            
         $dbname="pkp";

         $bledneHaslo = 0;
         $nieMaKonta = 0;

         $login=$_POST["login"];
         $haslo=$_POST["haslo"];

         try{
            $polaczenie=mysqli_connect($dbhost, $dbuser, $dbpass, $dbname);

        }
        catch(Exception $e){
            header("Location: index.php");
        }




         $result = mysqli_query($polaczenie, "SELECT log FROM users WHERE log='$login'");
         $row=mysqli_fetch_assoc($result);

         if(empty($row["log"])){

             $nieMaKonta = 1;
             mysqli_free_result($result);
         }else{

             $result2 = mysqli_query($polaczenie, "SELECT ID, log, haslo FROM users WHERE log='$login' AND haslo='$haslo'");
             $row=mysqli_fetch_assoc($result2);

             if(empty($row["log"])){

                $bledneHaslo = 1;
                mysqli_free_result($result);
                mysqli_free_result($result2);

             }else{

                 
                $_SESSION["ID"] = $row["ID"];
                $_SESSION["username"] = $row["log"];

                header("Location: index.php");

                mysqli_free_result($result);
                mysqli_free_result($result2);
                mysqli_close($polaczenie);

             }

         }

     }

?>  

    <html class="ht1">
     <head>
         <title>Zaloguj się do <?php
                
                $FileTOread = @fopen($nameOFpageNameFile, "r");

                if(!$FileTOread){

                  echo "TrainaTon.pl";

                }else{

                  echo fread($FileTOread, filesize($nameOFpageNameFile));
                  fclose($FileTOread);

                }

            ?></title>
         <meta charset="utf-8">
         <link rel="stylesheet" href="PKP.css">
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
            <img src="
                
                <?php
                
                $FileTOread = @fopen($nameOFlogoPathFile, "r");

                if(!$FileTOread){

                  echo "graphics/logoTT.png";

                }else{

                  echo fread($FileTOread, filesize($nameOFlogoPathFile));

                }
  
                ?>
                " alt="Logo Firmy TrainaTon" class="logo">
            </a>


             <nav class="nawigacja">

                 <a href="index.php" class="nawigacjaPrzyciski">
                     <img src="graphics/StronaGlowna.png" alt="dom" class="ikonyPrzyciskow">Strona Główna
                 </a>

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

         <section class="panelLogowania">       <!-- panel logowania -->

             <div  class="obszarlogo">
                 <img src="
                 <?php
                
                    $FileTOread = @fopen($nameOFlogoPathFile, "r");

                    if(!$FileTOread){

                      echo "graphics/logoTT.png";

                    }else{

                      echo fread($FileTOread, filesize($nameOFlogoPathFile));

                    }
  
                ?>
                " alt="Logo Firmy TrainaTon"  class="logoLogin">

             </div>

             <h2>Logowanie</h2><br>

             <form method="POST" action="logowanie.php">
                 <?php
                 if (!empty($_POST["login"]) && $nieMaKonta == 1 ){

                     echo "<h2 style='margin-top: 0%; color: red;'>Nie ma takiego konta</h2><br> <a href='logowanie.php'><button class='PrzyciskZaloguj'>Spróbuj ponownie</button></a><br>";

                 }elseif(isset($_POST["login"]) && $bledneHaslo == 1 ){

                     echo "<input type='text' placeholder='Podaj login' name='login' class='Polelogin'><br>";
                     echo '<input type="password" placeholder="Błędne hasło" name="haslo" class="PoleloginError">';

                 }else{

                     echo "<input type='text' placeholder='Podaj login' name='login' class='Polelogin'><br>";
                     echo '<input type="password" placeholder="Podaj hasło" name="haslo" class="Polelogin">';

                 }


                 ?>

                 <button type="submit" class="PrzyciskZaloguj">Zaloguj się</button>
             </form>

             <h4>Nie masz konta?</h4>

             <a href="rejestracja.php">
                 <button class="PrzyciskZarejestruj">Zarejestruj się</button>
             </a>

         </section>

     </body>






    </html>








<?php
}else{
?>








    <html>
     <head>
         <title>Twoje Konto w <?php
                
                $FileTOread = @fopen($nameOFpageNameFile, "r");

                if(!$FileTOread){

                  echo "TrainaTon.pl";

                }else{

                  echo fread($FileTOread, filesize($nameOFpageNameFile));
                  fclose($FileTOread);

                }

            ?></title>
         <meta charset="utf-8">
         <link rel="stylesheet" href="PKP.css">
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

     <body class="bodyKonto">


         <header class="pasekGorny">

            <a  class="LogoContainer" href="index.php">
            <img src="
                
                <?php
                
                $FileTOread = @fopen($nameOFlogoPathFile, "r");

                if(!$FileTOread){

                  echo "graphics/logoTT.png";

                }else{

                  echo fread($FileTOread, filesize($nameOFlogoPathFile));

                }
  
                ?>
                " alt="Logo Firmy TrainaTon" class="logo">
            </a>


             <nav class="nawigacja">

                 <a href="index.php" class="nawigacjaPrzyciski">
                     <img src="graphics/StronaGlowna.png" alt="dom" class="ikonyPrzyciskow">Strona Główna
                 </a>

                    <?php

                        $dbhost="localhost";    
                        $dbuser="root";         
                        $dbpass="";            
                        $dbname="pkp";

                        $IDzalogowanego = $_SESSION["ID"];

                        try{
                            $polaczenie=mysqli_connect($dbhost, $dbuser, $dbpass, $dbname);

                        }
                        catch(Exception $e){
                            header("Location: index.php");
                        }


                        $result = mysqli_query($polaczenie, "SELECT rola, log FROM users WHERE ID=$IDzalogowanego");
                        $row=mysqli_fetch_assoc($result);

                        if(isset($row["rola"])){
                            if($row["rola"] == "admin"){

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

         

         <section class="panelKonto">
         
            <div class="srodekPanelKonto">

                <table>

                    <tr>

                        <td class="srodekPanelKontoLewy">

                            <h2>Zalogowano jako:

                            <?php

                                echo $row["log"];

                            ?>

                            </h2>

                            <a href="logout.php">
                                <button class="PrzyciskWyloguj">Wyloguj się</button>
                            </a>

                        </td>

                        <td class="srodekPanelKontoPrawy">

                        </td>

                    </tr>

                </table>

            </div>

        </section>

    </html>




<?php
}
?>