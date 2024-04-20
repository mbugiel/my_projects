<?php

    $nameOFlogoPathFile = "uploads/logopath.txt";
    $nameOFpageNameFile = "uploads/pagename.txt";
    $nameOFIcoPathFile = "uploads/Icopath.txt";
    $nameOFsloganFile = "uploads/slogan.txt";
    $nameOFnoteFile = "uploads/pagenote.txt";

    if(isset($_POST["login"]) && isset($_POST["haslo"]) && isset($_POST["haslo2"]) ){

        $dbhost="localhost";    
        $dbuser="root";         
        $dbpass="";            
        $dbname="pkp";

        $takiLoginIstnieje = 0;
        $hasloDoPoprawy = 0;
        $DaneWstawione = 0;
        
        try{
            $polaczenie=mysqli_connect($dbhost, $dbuser, $dbpass, $dbname);

        }
        catch(Exception $e){
            header("Location: index.php");
        }

        $login=$_POST["login"];
        
        $result = mysqli_query($polaczenie, "SELECT log FROM users WHERE log='$login'");
        $row = mysqli_fetch_assoc($result);

        if(!empty($row["log"])){
            $takiLoginIstnieje = 1;
            mysqli_free_result($result);
        }else{

            mysqli_free_result($result);

            if($_POST["haslo"] != $_POST["haslo2"]){

                $hasloDoPoprawy = 1;
                

            }else{

                $result2 = mysqli_query($polaczenie, "SELECT ID FROM users WHERE ID>0");
                $row2 = mysqli_fetch_assoc($result2);
                $haslo = $_POST["haslo"];
                
                if(!empty($row2["ID"])){ 
                    
                    mysqli_query($polaczenie, "INSERT INTO users VALUES ('', 'user', '$login', '$haslo')");
                    $DaneWstawione = 1;

                }else{
                    mysqli_query($polaczenie, "INSERT INTO users VALUES ('', 'admin', '$login', '$haslo')");
                    $DaneWstawione = 1;
                }

                mysqli_free_result($result2);
               
            }

            
            
        }
        
       

        



    }

?>

<html class="ht1">
    <head>
        <title>Zarejestruj się w <?php
                
                $FileTOread = @fopen($nameOFpageNameFile, "r");

                if(!$FileTOread){

                  echo "TrainaTon.pl";

                }else{

                  echo fread($FileTOread, filesize($nameOFpageNameFile));
                  fclose($FileTOread);

                }

            ?>
            </title>
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
            " alt="Logo Firmy TrainaTon" class="logoLogin">
            </div>

            <h2>Rejestracja</h2><br>

                <?php

                    if(isset($_POST["login"]) && $DaneWstawione == 1){

                        echo "<div style='width:70%; margin-left: 15%;'><h2 style='color:green;'>Pomyślnie zarejestrowano konto w serwisie TrainaTon!</h2></div>";
                        echo "<a href='logowanie.php'><button class='PrzyciskZaloguj'>Zaloguj się</button></a>";
                    }else{

                ?>

            <form method="POST" action="rejestracja.php">

                <?php
                if(isset($_POST["login"]) && $takiLoginIstnieje == 1){

                    echo '<input type="text" required pattern=".{3,}" title="Login musi mieć co najmniej 3 znaki" placeholder="Ten login jest zajęty, wprowadź inny" name="login" class="PoleloginError"><br>';
                    echo '<input type="password" required pattern=".{8,}" title="Hasło musi mieć co najmniej 8 znaków" placeholder="Podaj hasło" name="haslo" class="Polelogin">';
                    echo '<input type="password" required pattern=".{8,}" placeholder="Powtórz hasło" name="haslo2" class="Polelogin">';

                }elseif(isset($_POST["login"]) && $hasloDoPoprawy == 1){

                    echo '<input type="text" required pattern=".{3,}" title="Login musi mieć co najmniej 3 znaki" placeholder="Podaj login" name="login" class="Polelogin"><br>';
                    echo '<input type="password" required pattern=".{8,}" title="Hasło musi mieć co najmniej 8 znaków" placeholder="Podaj hasło" name="haslo" class="Polelogin">';
                    echo '<input type="password" required pattern=".{8,}" placeholder="Błąd, powtórz hasło jeszcze raz" name="haslo2" class="PoleloginError">';

                }else{

                    echo '<input type="text" required pattern=".{3,}" title="Login musi mieć co najmniej 3 znaki" placeholder="Podaj login" name="login" class="Polelogin"><br>';
                    echo '<input type="password" required pattern=".{8,}" title="Hasło musi mieć co najmniej 8 znaków" placeholder="Podaj hasło" name="haslo" class="Polelogin">';
                    echo '<input type="password" required pattern=".{8,}" placeholder="Powtórz hasło" name="haslo2" class="Polelogin">';
                }                
                ?>
                
                <button type="submit" class="PrzyciskZaloguj">Zarejestruj się</button>


            </form>

            <a href="logowanie.php" class="powrotZaloguj">
                Zaloguj się
            </a>

            <?php
                    }
            ?>


        </section>

    </body>






</html>