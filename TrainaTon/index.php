<?php
    session_start();

    $dbhost="localhost";    
    $dbuser="root";         
    $dbpass="";            
    $dbname="pkp";
    $polaczenie=mysqli_connect($dbhost, $dbuser, $dbpass);

    $nameOFlogoPathFile = "uploads/logopath.txt";   //in use
    $nameOFpageNameFile = "uploads/pagename.txt";   //in use
    $nameOFIcoPathFile = "uploads/Icopath.txt"; //in use
    $nameOFsloganFile = "uploads/slogan.txt";   //in use
    $nameOFnoteFile = "uploads/pagenote.txt";   //in use

if($polaczenie){

    { /*Odbudowa bazy danych*/
    mysqli_query($polaczenie,"CREATE DATABASE IF NOT EXISTS `pkp` DEFAULT CHARACTER SET utf8 COLLATE utf8_polish_ci;");
    mysqli_query($polaczenie,"USE `pkp`;");
    mysqli_query($polaczenie,"CREATE TABLE IF NOT EXISTS `posts` (
        `ID` int(11) NOT NULL,
        `userID` int(11) NOT NULL,
        `data` datetime NOT NULL,
        `nr_pociagu` int(11) NOT NULL,
        `nazwisko_maszynisty` text NOT NULL,
        `data_odjazdu` date NOT NULL,
        `czas_dojazdu` time NOT NULL,
        `opoznienie` int(11) NOT NULL,
        `przewoznik` text NOT NULL
      ) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_polish_ci;");

    mysqli_query($polaczenie,"CREATE TABLE IF NOT EXISTS `users` (
        `ID` int(11) NOT NULL,
        `rola` text NOT NULL,
        `log` text NOT NULL,
        `haslo` text NOT NULL
      ) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_polish_ci;");

    mysqli_query($polaczenie,"CREATE TABLE IF NOT EXISTS `comments` (
        `ID` int(11) NOT NULL,
        `userID` int(11) NOT NULL,
        `postID` int(11) NOT NULL,
        `tekst` text NOT NULL
      ) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_polish_ci;");
    
    mysqli_query($polaczenie,"ALTER TABLE `posts`
    ADD PRIMARY KEY IF NOT EXISTS (`ID`);");
    
    mysqli_query($polaczenie,"ALTER TABLE `users`
    ADD PRIMARY KEY IF NOT EXISTS (`ID`);");

    mysqli_query($polaczenie,"ALTER TABLE `comments`
    ADD PRIMARY KEY IF NOT EXISTS (`ID`);");

    mysqli_query($polaczenie,"ALTER TABLE `posts`
    MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;");

    mysqli_query($polaczenie,"ALTER TABLE `users`
    MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;");

    mysqli_query($polaczenie,"ALTER TABLE `comments`
    MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;");

    mysqli_query($polaczenie,"COMMIT;");
    }


    function CzyBylTakiPost($conn, $nrPociagu=NULL, $przewoznik=NULL, $DataOdj=NULL, 
                                                        $czas=NULL){



            $result = mysqli_query($conn, "SELECT * FROM posts"); /*wybieram wszystko z tabeli*/
            
            $CzyBylTakiPost = true;

            while($row = mysqli_fetch_assoc($result)){

                if($row["nr_pociagu"] == $nrPociagu && $row["przewoznik"] == $przewoznik &&
                    $row["data_odjazdu"] == $DataOdj && $row["czas_dojazdu"] == $czas){ /* sprawdzam czy już był taki post*/

                        echo "<script>alert('Post o tym spóźnieniu już istnieje! Wprowadź inne dane.')</script>";
                        mysqli_free_result($result);
                        return true;

                }else{
                    $CzyBylTakiPost = false;
                }

            }
            mysqli_free_result($result);
            return $CzyBylTakiPost;


    }

    function IDNajnowszegoPosta($conn){

        $result = mysqli_query($conn, "SELECT max(ID) FROM posts");  /*---sprawdzam id najnowszego posta----*/
            $row = mysqli_fetch_assoc($result);
    
    
            if($row["max(ID)"] != NULL){
    
                $najnowszeIDposta = $row["max(ID)"];
                mysqli_free_result($result);
                return $najnowszeIDposta;
            }else{
    
                $najnowszeIDposta = "brakPostow";
                mysqli_free_result($result);
                return $najnowszeIDposta;
            }
    
    }

    if(isset($_POST["dodajPost"])){

        $userID = $_SESSION["ID"];

        $dataWpisu = $_POST["data"];
        $nrPociagu = $_POST["trainNo"];
        $nazwisko = $_POST["maszynista"];
        $przwoznik = $_POST["przewoznik"];
        $DataOdj = $_POST["dataOdj"];
        $czas = $_POST["czas"];
        $opoznienie = $_POST["opoznienie"];

        if(IDNajnowszegoPosta($polaczenie) == "brakPostow" || CzyBylTakiPost($polaczenie, $nrPociagu, $przwoznik, $DataOdj, $czas) == false) {

            mysqli_query($polaczenie, 
                                "INSERT INTO posts VALUES 
                                ('', $userID, '$dataWpisu', $nrPociagu, '$nazwisko', '$DataOdj', '$czas', $opoznienie,  '$przwoznik')"
                            );

        }

        

    }

    if(isset($_POST["komentarz"])){

        $userID = $_SESSION["ID"];

        $IDposta = $_POST["IDposta"];
        $tekst = $_POST["komentarz"];

        mysqli_query($polaczenie, 
                                "INSERT INTO comments VALUES 
                                ('', $userID, '$IDposta', '$tekst')"
                            );

    }

    if(isset($_POST["usun"])){
        $IDdoUsun = $_POST["IDposta"];
        mysqli_query($polaczenie, "DELETE FROM posts WHERE ID = $IDdoUsun");
        mysqli_query($polaczenie, "DELETE FROM comments WHERE postID = $IDdoUsun");
    }

    if(isset($_POST["edytujKomentarz"])){
        
        $IDkomZmiana = $_POST["IDkomDoZmiany"];
        $nowaTresc = $_POST["nowyKom"];

        mysqli_query($polaczenie, "UPDATE `comments` SET `tekst` = '$nowaTresc' WHERE `ID` = $IDkomZmiana");
    }

    if(isset($_POST["usunKomentarz"])){
        $IDKomentarzaUsun = $_POST["IDkom"];
        mysqli_query($polaczenie, "DELETE FROM comments WHERE ID = $IDKomentarzaUsun");
    }

    if(isset($_POST["UpdatePosta"])){
        $IDpostaToUpdate = $_POST["IDpostUpdate"];
        $dataWpisu = $_POST["data"];
        $nrPociagu = $_POST["trainNo"];
        $nazwisko = $_POST["maszynista"];
        $DataOdj = $_POST["dataOdj"];
        $czas = $_POST["czas"];
        $opoznienie = $_POST["opoznienie"];
        $przwoznik = $_POST["przewoznik"];

        mysqli_query($polaczenie, "UPDATE `posts` SET `data` = '$dataWpisu', `nr_pociagu` = '$nrPociagu', 
                                `nazwisko_maszynisty` = ' $nazwisko', `data_odjazdu` = '$DataOdj', `czas_dojazdu` = '$czas', 
                                `opoznienie` = '$opoznienie', `przewoznik` = '$przwoznik' WHERE `posts`.`ID` = $IDpostaToUpdate");
    }
    function PokazWpisow($conn){

        $result = mysqli_query($conn, "SELECT * FROM posts ORDER BY data DESC");
        
        if(isset($_SESSION["ID"])){
            $userID = $_SESSION["ID"];
            $result2 = mysqli_query($conn, "SELECT rola FROM users WHERE ID=$userID");
            $row2 = mysqli_fetch_assoc($result2);
        }

        if (mysqli_num_rows($result) > 0){

            echo "<div style='height: 40px;'></div>";

            while($row  = mysqli_fetch_assoc($result)){
                $IDposta = $row["ID"];
                $dataWpisu = substr($row["data"], 0, 10);
                $nrPociagu = $row["nr_pociagu"];
                $nazwisko = $row["nazwisko_maszynisty"];
                $przwoznik = $row["przewoznik"];
                $DataOdj = $row["data_odjazdu"];
                $czas = $row["czas_dojazdu"];
                $opoznienie = $row["opoznienie"];

                echo "<div class='wpisy'>

                    <div class='UserElements'>
                        <section class='LeweInformacje'>

                            <section class='TrainInfo'>
                                <h3 class='kolorBlack flex_g1'>Pociąg nr <span class='kolorTrainaton'>$nrPociagu</span>
                                 przewoźnika <span class='kolorTrainaton'>$przwoznik</span></h3>
                                <h4 class='flex_g1 margin_top_20'>Odjechał w dniu <span class='kolorBlack'>$DataOdj</span></h4>
                                <h4 class='flex_g1'>Dojechał o godz. <span class='kolorBlack'>$czas</span></h4>
                                <h4 class='flex_g1'>Prowadził <span class='kolorBlack'>$nazwisko</span></h4>
                                <h3 class='flex_g1 text_align_left'>Spóźniony o <span class='kolorRed'>$opoznienie</span> min</h3>
                            </section>

                            <section class='data_kafelek'>
                                <div class='text_align_right margin_right_10 dataWpisu_kafelek'>$dataWpisu</div>
                            </section>

                        </section>

                        <section class='komentarzeSekcja'>
                            <h3 class='kolorBlack'>Komentarze</h3>

                            <div class='pokazKomentarzy'>";
                                pokazKomentarzy($conn, $IDposta);


                                echo "
                            </div>

                            <form method='post' action='index.php'>";

                        if(isset($_SESSION["ID"])){

                            echo "
                                <input class='inputKomentarzy' type='text' required placeholder='Napisz Tutaj...' name='komentarz'>
                                <input type='hidden' value='$IDposta' name='IDposta'>
                                <input class='buttonKomentarzy' type='submit' value='Opublikuj'>";

                        }else{
                            echo "
                                <input class='inputKomentarzy' type='text' disabled placeholder='Zaloguj Się Aby Dodać'>
                                <input class='buttonKomentarzy' type='submit' disabled value='Opublikuj'>";
                        }

                        echo"
                            </form>

                        </section>
                    </div>";

                    
                    if(isset($row2["rola"]) && ($row2["rola"] == "admin" || $row2["rola"] == "admin2")){
                        echo "
                        <div class='AdminElements'>

                            <form method='post' action='index.php'>
                            
                                <button type='submit' class='EdytujIusunWpis edytuj' name='edytuj'>Edytuj</button>
                                <input type='hidden' name='IDposta' value=$IDposta>
                                <button type='submit' class='EdytujIusunWpis usun' name='usun'>Usuń</button>

                            </form>
                        </div>";

                    }

                    echo "
                    
                </div>";
            }
            if(isset($result2)){
                mysqli_free_result($result2);
            }
        }

        mysqli_free_result($result);
    }

    function EdycjaWpisow($conn, $IDposta){
        $result = mysqli_query($conn, "SELECT * FROM posts WHERE ID=$IDposta");
        $row = mysqli_fetch_assoc($result);

        $data = str_replace(' ', 'T', $row["data"]);
        $TrainNo = $row["nr_pociagu"];
        $nazwisko = $row["nazwisko_maszynisty"];
        $DataOdj = $row["data_odjazdu"];
        $czas = $row["czas_dojazdu"];
        $opoznienie = $row["opoznienie"];
        $przwoznik = $row["przewoznik"];
        $IDtoUpdt = $IDposta;

        echo '
            <h3 class="padding20p">Edytujesz wpis z dnia '.substr($data, 0, 10).' dotyczący pociągu nr '.$TrainNo.' przewoźnika '.$przwoznik.'</h3>
            <form class="formEdycyjny" method="post" action="index.php">
        
            <a href="index.php"  class="flex_g1 inputContainer"><button class="PrzyciskAnulujIZapisz red">Anuluj</button></a>
                

            <div class="inputContainer flex_g1">  

                <label>
                    <span class="InputFont">Data Wpisu</span><br>
                    <input class="bgColorWH inputProperties czas_i_daty" type="datetime-local" name="data" required value='.$data.' >
                </label>

                <label>
                    <span class="InputFont">Data Odjazdu</span><br>
                    <input class="inputProperties czas_i_daty bgColorWH" type="date" name="dataOdj" required value='.$DataOdj.'>
                </label>

                <label>
                    <span class="InputFont">Czas Dojazdu</span><br>
                    <input class="inputProperties czas_i_daty bgColorWH" type="time" name="czas" required value='.$czas.'>
                </label>

                <label>
                    <span class="InputFont">Opóźnienie</span><br>
                    <input class="inputProperties Rplaceholder bgColorWH" type="number" name="opoznienie" value='.$opoznienie.' required min="0" placeholder="min">
                </label>

                <label>
                    <span class="InputFont">Numer Pociągu</span><br>
                    <input class="inputProperties bgColorWH" type="number" name="trainNo" value='.$TrainNo.' required min="1" placeholder="...">
                </label>

                <label>
                    <span class="InputFont">Nazwa Przewoźnika</span><br>
                    <input class="inputProperties bgColorWH" type="text" name="przewoznik" value='.$przwoznik.' required placeholder="...">
                </label>

                <label>
                    <span class="InputFont">Nazwisko Maszynisty</span><br>
                    <input class="inputProperties bgColorWH" type="text" name="maszynista" value='.$nazwisko.' required placeholder="...">
                </label>

            </div>

                <input type="number" hidden name="IDpostUpdate" value='.$IDtoUpdt.'>

            <div class="flex_g1 inputContainer">
                <input class="PrzyciskAnulujIZapisz green" type="submit" name="UpdatePosta" value="Zapisz Zmiany">
            </div>


            </form>
            ';

            mysqli_free_result($result);
        
    }

    function EdycjaKomentarzy($conn, $IDkom){
        
        $result = mysqli_query($conn, "SELECT * FROM comments WHERE ID=$IDkom");
        $row = mysqli_fetch_assoc($result);

        $IDpostaWkom = $row["postID"];
        $IDtoUpdateKom = $IDkom;
        $tresc = $row["tekst"];
        $autorID = $row["userID"];

        $result2 = mysqli_query($conn, "SELECT `log` FROM users WHERE ID=$autorID");
        $row2 = mysqli_fetch_assoc($result2);

        $autor = $row2["log"];

        $result3 = mysqli_query($conn, "SELECT * FROM posts WHERE ID=$IDpostaWkom");
        $row3 = mysqli_fetch_assoc($result3);
        
        $data = str_replace(' ', 'T', $row3["data"]);
        $TrainNo = $row3["nr_pociagu"];
        $przwoznik = $row3["przewoznik"];

        

        echo '<h3 class="padding20p">Edytujesz komentarz użytkownika '.$autor.' przy wpisie z dnia '.substr($data, 0, 10).' dotyczącego pociągu nr '.$TrainNo.' przewoźnika '.$przwoznik.'</h3>

                <form class="formEdycyjny" action="index.php" method="post">
                    <a href="index.php" class="inputContainer  flex_g1">
                        <button class="PrzyciskAnulujIZapisz red">Anuluj</button>
                    </a>
                    
                    <div class="KomentarzArea">
                        <textarea class="textarea" name="nowyKom" required placeholder="Napisz komentarz...">'.$tresc.'</textarea>
                    </div>

                    <input type="hidden" name="IDkomDoZmiany" value='.$IDtoUpdateKom.'>
                    
                    <div class="flex_g1 inputContainer">
                        <input class="PrzyciskAnulujIZapisz green" type="submit" name="edytujKomentarz" value="Zapisz zmiany">
                    </div>
                </form>';

                mysqli_free_result($result);
                mysqli_free_result($result2);
                mysqli_free_result($result3);
                
    }
    function PokazKomentarzy($conn, $postID){

        $result = mysqli_query($conn, "SELECT * FROM comments WHERE postID=$postID");

        if (mysqli_num_rows($result) > 0){

            echo "<ul>";
            while($row  = mysqli_fetch_assoc($result)){
                $IDkomentarza = $row["ID"];
                $IDusera = $row["userID"];
                $tresc = $row["tekst"];
                
                $result2 = mysqli_query($conn, "SELECT log FROM users WHERE ID=$IDusera");
                $row2  = mysqli_fetch_assoc($result2);
                $LoginPiszacego = $row2["log"];

                echo "<li class='komentarzeUL'><span class='username'>$LoginPiszacego:</span> $tresc ";
                
                if(isset($_SESSION["ID"])){
                    
                    $zalogowanyID = $_SESSION["ID"];

                    $result3 = mysqli_query($conn, "SELECT rola FROM users WHERE ID=$zalogowanyID");
                    $row3 = mysqli_fetch_assoc($result3);

                    if($row3["rola"] == "admin" || $row3["rola"] == "admin2" || $zalogowanyID == $IDusera){

                        echo "
                        <form class='formEdycjaKom' method='post' action='index.php'>
                            <input type='hidden' name='IDkom' value='$IDkomentarza'>
                            <input class='EdytujUsunKom edytujKom' type='submit' name='edytujKomentarzFunkcja' value='Edytuj'>
                            <input class='EdytujUsunKom usunKom' type='submit' name='usunKomentarz' value='Usuń'>
                        </form>";

                    }
                    mysqli_free_result($result3);
                }

                echo "</li>";
            }
            echo "</ul>";
            mysqli_free_result($result2);
        }

        mysqli_free_result($result);
    }

    function NajwiekszeOpoznienie($conn){

        $result1 = mysqli_query($conn, "SELECT max(opoznienie) FROM posts");
        $row1 = mysqli_fetch_assoc($result1);

        if ($row1["max(opoznienie)"] != NULL){
            
            $maxDelay = $row1["max(opoznienie)"];

            $result1 = mysqli_query($conn, "SELECT * FROM posts WHERE opoznienie=$maxDelay");
            $row1 = mysqli_fetch_assoc($result1);


                $IDposta = $row1["ID"];
                $nrPociagu = $row1["nr_pociagu"];
                $przewoznik = $row1["przewoznik"];
                $DataOdj = $row1["data_odjazdu"];
                $opoznienie = $row1["opoznienie"];

                echo "Pociąg nr $nrPociagu przewoźnika \"$przewoznik\" pobił rekord opóźnienia! W dniu $DataOdj spóźnił się aż o $opoznienie min!";

        }else{
            echo "Nie ma żadnych wpisów w bazie danych!";
        }
        mysqli_free_result($result1);

    }

    function ilewpisow ($conn){
        
        $result = mysqli_query($conn, "SELECT ID FROM posts");

        $iloscWynikow=mysqli_num_rows($result);

        echo $iloscWynikow;

        mysqli_free_result($result);
    }

?>

<html class="ht1">
    <head>
        <title><?php
                
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
            <img src="
                
                <?php
                
                    $FileTOread = @fopen($nameOFlogoPathFile, "r");

                    if(!$FileTOread){

                      echo "graphics/logoTT.png";

                    }else{

                      echo fread($FileTOread, filesize($nameOFlogoPathFile));
                      fclose($FileTOread);

                    }
  
                ?>
                " alt="Logo Firmy TrainaTon" class="logo">
            </a>
            

            <nav class="nawigacja">

                <a href="index.php" class="nawigacjaPrzyciski">
                    <img src="graphics/StronaGlowna.png" alt="dom" class="ikonyPrzyciskow">Strona Główna
                </a>

                <?php /*------Jeżeli jesteś adminem to masz zakładkę panel admina---------*/

                    if(isset($_SESSION["ID"])){

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
                    }

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

        <section class="dolnaStrona">
        
            <div class="pasekZopoznieniem">
                    <marquee class="najwiekszeOpoznienie">
                        <?php
                            NajwiekszeOpoznienie($polaczenie);
                        ?>
                    </marquee>
            </div>
                
            
            <div class="sekcjaLewa">
                    
                    <div class="BlokadaFlexa"> <!-- Blokada przed powiększaniem się diva zamiast overflow scroll -->

                        <!-- krótka notka o stronie---->
                        <section class="LewaGora">

                                <div class="StronaOpis">
                                        
                                        <img src="
                                        <?php
                
                                        $FileTOread = @fopen($nameOFlogoPathFile, "r");

                                        if(!$FileTOread){

                                          echo "graphics/logoTT.png";

                                        }else{

                                          echo fread($FileTOread, filesize($nameOFlogoPathFile));
                                          fclose($FileTOread);

                                        }
  
                                        ?>
                                        " alt="Logo Firmy TrainaTon" class="logoTekst"> 

                                        <?php
                
                                        $FileTOread = @fopen($nameOFnoteFile, "r");

                                        if(!$FileTOread){
                                        
                                          echo "to idealne miejsce aby podzielić się swoim doświadczeniem związanym ze środkiem transportu 
                                                    jakim jest kolej. Napisz innym co sądzisz o spóźnieniu Twojego pociągu!";
                                        
                                        }else{

                                          echo fread($FileTOread, filesize($nameOFnoteFile));
                                          fclose($FileTOread);
                                        
                                        }
                                    
                                        ?>

                                </div>

                        </section>

                    <?php
                        if(!isset($_SESSION["ID"])){                /* jeżeli nie zalogowano to info o tym żeby się zalogować-------------------*/
                    ?>
                        <section class="LewyDol">
                                
                            <h2 class="ZalogujAbyDodacWpis">Aby skomentować wpis musisz się zalogować</h2>
                                

                            <a href="logowanie.php">
                                    <button class="PrzyciskZaloguj">Zaloguj się</button>
                            </a>

                                
                            <h4 class="ZalogujAbyDodacWpis">Nie masz konta?</h4>
                                

                            <a href="rejestracja.php">
                                    <button class="PrzyciskZarejestruj">Zarejestruj się</button>
                            </a>

                        </section>

                    <?php
                    }else{                                               /*KONIEC jeżeli nie zalogowano i POCZĄTEK zalogowano-------------------*/
                            $userID = $_SESSION["ID"];
                            $wynik = mysqli_query($polaczenie, "SELECT rola FROM users WHERE ID=$userID");
                            $rowWynik = mysqli_fetch_assoc($wynik);

                            if($rowWynik["rola"] == "admin" || $rowWynik["rola"] == "admin2"){               /* Jeżeli jesteś adminem to możesz wstawiać posty-----*/

                                if(IDNajnowszegoPosta($polaczenie) != "brakPostow"){

                                    /*gdy są posty to zmienne uzupełniają się najnowszym postem i 
                                    uzupełniają sobą formularz*/
                                    
                                    
                                    $NajnowszyPostID = IDNajnowszegoPosta($polaczenie);
                                        
                                    $result3 = mysqli_query($polaczenie, "SELECT * FROM posts WHERE ID=$NajnowszyPostID");
                                    $row3 = mysqli_fetch_assoc($result3);
                                        
                                    $nrPociagu = $row3["nr_pociagu"];                       /*ustawione zmienne dla formularza*/
                                    $nazwisko = $row3["nazwisko_maszynisty"];
                                    $DataOdj = $row3["data_odjazdu"];
                                    $czas = $row3["czas_dojazdu"];
                                    $opoznienie = $row3["opoznienie"];
                                    $przwoznik = $row3["przewoznik"];

                                }else{

                                    $nrPociagu = "...";            /*gdyby nie było postów zmienne są takie*/
                                    $nazwisko = "";
                                    $DataOdj = "";
                                    $czas = "";
                                    $opoznienie ="";
                                    $przwoznik = "";
                                    
                                }
                                
                                if(isset($result3)){
                                    mysqli_free_result($result3);
                                }


                    ?>
                        <!-- formularz ze zmiennymi które są powyżej----->
                        <section class="LewyDolForm">                   
                                <h2 class="ZalogujAbyDodacWpis">Opublikuj post tutaj:</h2>

                                <form class="form" action="index.php" method="POST">

                                    <!--<label>-->
                                        <!--<span class="InputFont">Data Wpisu</span><br>-->
                                        <input type="datetime-local" name="data" hidden value="<?php echo date("Y-m-d")."T".date("H:i"); ?>">
                                    <!--</label>-->

                                    <label>
                                        <span class="InputFont">Data Odjazdu</span><br>
                                        <input class="inputProperties bgColorSzary czas_i_daty" type="date" name="dataOdj" min="0001-01-01" max="9999-12-31" required value="<?php echo $DataOdj; ?>">
                                    </label>

                                    <label>
                                        <span class="InputFont">Czas Dojazdu</span><br>
                                        <input class="inputProperties bgColorSzary czas_i_daty" type="time" name="czas" required value="<?php echo $czas; ?>">
                                    </label>

                                    <label>
                                        <span class="InputFont">Opóźnienie</span><br>
                                        <input class="inputProperties bgColorSzary Rplaceholder" type="number" name="opoznienie" required min="0" placeholder="min" value="<?php echo $opoznienie; ?>">
                                    </label>

                                    <label>
                                        <span class="InputFont">Numer Pociągu</span><br>
                                        <input class="inputProperties bgColorSzary" type="number" name="trainNo" required min="1" placeholder="..." value="<?php echo $nrPociagu; ?>">
                                    </label>

                                    <label>
                                        <span class="InputFont">Nazwa Przewoźnika</span><br>
                                        <input class="inputProperties bgColorSzary" type="text" name="przewoznik" required placeholder="..." value="<?php echo $przwoznik; ?>">
                                    </label>

                                    <label>
                                        <span class="InputFont">Nazwisko Maszynisty</span><br>
                                        <input class="inputProperties bgColorSzary" type="text" name="maszynista" required placeholder="..." value="<?php echo $nazwisko; ?>">
                                    </label>

                                    <input class="PrzyciskOpublikuj" type="submit" name="dodajPost" value="Opublikuj">

                                </form>

                        </section>

                        <?php
                            }else{    /*------------------Jeżeli zwykłym userem to masz info gdzie komentować---*/
                        ?>

                        <section class="LewyDol">
                                
                            <h2 class="ZalogujAbyDodacWpis">Aby skomentować wpisy znajdź tę opcję przy każdym z nich</h2>
    
                        </section>

                        <?php
                                }
                                mysqli_free_result($wynik);
                            }                                                    /*KONIEC jeżeli zalogowano -------------------*/
                        ?>

                        <section class="ileWpisow">Ilość wpisów na stronie: <?php ilewpisow($polaczenie) ?></section>
 
                    </div>

            </div>

            <div class="sekcjaSrodkowa"> <!-- -------------miejsce na wpisy --------->
                    <?php
                        
                        if(isset($_POST["edytuj"])){                /* ----------jeżeli edytujesz wpis to wchodzisz w panel edycji----------*/
                            echo "<div style='height: 40px;'></div>";
                            $IDdoEdycji = $_POST["IDposta"];

                    ?>

                            <section class="poleEdycjiWpisu">

                                <?php EdycjaWpisow($polaczenie, $IDdoEdycji); ?>

                            </section>

                    <?php
                        }elseif(isset($_POST["edytujKomentarzFunkcja"])){   /* ----------jeżeli edytujesz komentarz to wchodzisz w panel edycji----------*/
                            echo "<div style='height: 40px;'></div>";
                            $IDkom = $_POST["IDkom"];
                            
                    ?>    

                            <section class="poleEdycjiWpisu">

                                <?php EdycjaKomentarzy($polaczenie, $IDkom); ?>

                            </section>

                    <?php

                        }else{          /*------------jeżeli nie edytujesz to masz pokaz wpisów ----------*/
                            PokazWpisow($polaczenie);
                        }

                    ?>
            </div>

            <div class="sekcjaPrawa">

            </div>

              
            
      
        </section>





    </body>
</html>
<?php
}else{
?>

<html lang="pl">
<head>
        <title>TrainaTon.pl</title>
        <meta charset="utf-8">
        <link rel="stylesheet" href="StronaGlowna.css">
        <link rel="icon" href="graphics/LogoTrainaTon2.ico">
    </head>

    <body>
        Brak połączenia z bazą danych! Strona bez bazy danych nie działa.
    </body>



</html>


<?php
}
?>