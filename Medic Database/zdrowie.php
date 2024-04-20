<!DOCTYPE html>
<html lang="pl">

    <head>
        <title>PHP zdrowie</title>
    </head>

    <?php 
        if(isset($_POST["data"]) && isset($_POST["czas"]) && isset($_POST["imie"]) && isset($_POST["waga"]) && isset($_POST["wzrost"]) 
            && isset($_POST["temperatura"]) && isset($_POST["puls"]) && isset($_POST["cisnienie1"]) && isset($_POST["cisnienie2"]) 
            && isset($_POST["saturacja"]) && isset($_POST["stres"]) && isset($_POST["szklanki"])){                        // sprawdzam czy zmienne z formularza są ustawione

            $data = $_POST["data"];
            $czas = $_POST["czas"];
            $imie = $_POST["imie"];
            $waga = $_POST["waga"];
            $wzrost = $_POST["wzrost"];
            $temperatura = $_POST["temperatura"];   //ustawiam zmienne na podstawie tych z formularza
            $puls = $_POST["puls"];
            $cisnienie1 = $_POST["cisnienie1"];
            $cisnienie2 = $_POST["cisnienie2"];
            $saturacja = $_POST["saturacja"];
            $stres = $_POST["stres"];
            $szklanki = $_POST["szklanki"];


            //dane do połączenia z bazą:
            $dbhost="localhost";    //adres bazy	   
		    $dbuser="root";         //użytkownik logujący się do bazy      
		    $dbpass="";             //brak hasła
            $dbname="zdrowie";      //baza o nazwie zdrowie
            $dbtable = "informacje";
                  


            $polaczenie=mysqli_connect($dbhost, $dbuser, $dbpass, $dbname); //łączę się do bazy
		    
            if ($polaczenie){   //jeśli połączenie zostało pomyślnie ustawione to ma się wykonać to co w IF'ie
			    
                //wstawiam dane pacjenta do tabeli $dbtable (informacje) w bazie:
                mysqli_query($polaczenie, "INSERT INTO $dbtable VALUES('', '$imie', '$data','$czas',$waga,$wzrost,$temperatura,$puls,$cisnienie1,$cisnienie2,$saturacja,$stres,$szklanki)");
                
			    echo "Dane zostaly wstawione <br><br>";







                //mysqli_query daje zapytanie do bazy o wszystkie dane w tabeli
                $wynikZapytania = mysqli_query($polaczenie, "SELECT Imie, data, godzina, waga, wzrost, temperatura, puls, cisnienieSkurczowe, cisnienieRozkurczowe, saturacja, poziomStresu, iloscSzklanekWody FROM $dbtable");

                echo "Dane w bazie danych: <br>";
                if (mysqli_num_rows($wynikZapytania) > 0) {     //Jeśli jest więcej niż 0 rzędów danych w odpowiedzi z bazy to wykonuje się to co w IF'ie

                    
                    echo "<table border='1'>
                            <tr>
                                <td>Imię:</td> <td>data:</td> <td>godzina:</td> <td>waga w kg:</td> <td>wzrost w cm:</td> 
                                <td>temperatura w °C:</td> <td>puls:</td> <td>ciśnienie skurczowe:</td> 
                                <td>ciśnienie rozkurczowe:</td> <td>saturacja:</td> <td>poziomStresu 1-5:</td> 
                                <td>ilość szklanek wody:</td>
                            </tr>";

                    while ($row = mysqli_fetch_assoc($wynikZapytania)) {    //pętla wykonuje się dla każdego rzędu odpowiedzi z bazy. Dostęp do rzędu uzyskujemy dzięki funkcji mysql_fetch_assoc, która zamienia rząd odpowiedzi na array (w tym przypadku "$row"), do którego mamy dostęp
                        
                        $Imie = $row["Imie"];
                        $data = $row["data"];  //przypisuję wartości elementów z array'a do zmiennych
                        $godz = $row["godzina"];
                        $waga = $row["waga"];
                        $wzrost = $row["wzrost"];
                        $temp = $row["temperatura"];
                        $puls = $row["puls"];
                        $cisS = $row["cisnienieSkurczowe"];
                        $cisR = $row["cisnienieRozkurczowe"];
                        $saturacja = $row["saturacja"];
                        $poziom = $row["poziomStresu"];
                        $szklanki = $row["iloscSzklanekWody"];

                        echo "<tr>
                                <td>$Imie</td> <td>$data</td> <td>$godz</td> <td>$waga</td>         
                                <td>$wzrost</td> <td>$temp</td> <td>$puls</td> <td>$cisS</td>
                                <td>$cisR</td> <td>$saturacja</td> <td>$poziom</td> <td>$szklanki</td>
                             </tr>";    //wypisuję dane do tabeli
                    }

                    echo "</table> <br><br>";
                
                }
                mysqli_free_result($wynikZapytania); //czyszczę dane odpowiedzi z bazy

                
                
                
                
                
                
                
                //Zapytanie nr 1.:
                //mysqli_query daje zapytanie do bazy o wzrost najwyższej osoby spośród tych wyższych od 200 cm:
                //Może to służyć do tego żeby personel np szpitala wiedział czy potrzebuje nowej wagi lekarskiej z pomiarem wzrostu większym niż 200cm.
                $wynikZapytania = mysqli_query($polaczenie, "SELECT max(wzrost) FROM $dbtable WHERE wzrost>200");
            
                if(mysqli_num_rows($wynikZapytania)>0){     //Jeśli jest więcej niż 0 rzędów danych w odpowiedzi z bazy to wykonuje się to co w IF'ie
    
                    echo "Wzrost najwyższego pacjenta powyżej 200 cm: <br>";

                    $row = mysqli_fetch_assoc($wynikZapytania);     //zamieniam rząd odpowiedzi z bazy na array (funkcją mysqli_fetch_assoc), do którego będę miał dostęp
                    
                    $wzrostZbazy = $row["max(wzrost)"];   //przypisuję do zmiennej wartość "max(wzrost)" z array'a 
                    
                    if(empty($wzrostZbazy)){    //Sprawdzam czy $wzrostZbazy jest pusty, bo gdy nie ma pacjenta wyższego niż 150cm to jest pusty

                        echo "Brak pacjentów z wzrostem powyżej 200 cm <br><br>";    //Jeśli tak to ma się to wyświetlić

                    } else {    //Jeśli jednak coś zawiera to wypisuję to w tabeli

                        echo "<table border='1'>                        
                             <tr>
                                 <td>Największy wzrost:</td> <td>$wzrostZbazy</td>  
                            </tr>
                            </table>
                            <br><br>
                            ";
                    }

                }
                mysqli_free_result($wynikZapytania); //czyszczę dane odpowiedzi z bazy
                


                
                


                
                
                
                
                //Zapytanie nr 2.:
                //mysqli_query daje zapytanie do bazy o imię i temperaturę osoby, która ma 37 lub więcej stopni C oraz każe bazie w odpowiedzi zamienić nazwę kolumny "temperatura" na "temp"
                //Może to służyć do tego żeby personel wiedział, czy może wpuścić daną osobę na oddział w odwiedziny do osoby chorej.
                $wynikZapytania = mysqli_query($polaczenie, "SELECT Imie, temperatura AS temp FROM $dbtable WHERE temperatura>=37");

                echo "Osoby mające co najmniej stan podgorączkowy: <br>";
                if (mysqli_num_rows($wynikZapytania) > 0) {     //Jeśli jest więcej niż 0 rzędów danych w odpowiedzi z bazy to wykonuje się to co w IF'ie

                    
                    echo "<table border='1'>
                            <tr>
                                <td>Imię:</td> <td>Temperatura:</td>
                            </tr>";

                    while ($row = mysqli_fetch_assoc($wynikZapytania)) {    //pętla wykonuje się dla każdego rzędu odpowiedzi z bazy. Dostęp do rzędu uzyskujemy dzięki funkcji mysql_fetch_assoc, która zamienia rząd odpowiedzi na array (w tym przypadku "$row"), do którego mamy dostęp
                        
                        $Imie = $row["Imie"];   //przypisuję wartości elementów z array'a do zmiennych
                        $temp = $row["temp"];

                        echo "<tr>
                                <td>$Imie</td> <td>$temp</td>
                             </tr>";    //wypisuję dane do tabeli
                    }

                    echo "</table> <br><br>";
                
                }else{ //Jeśli nie będzie ani jednego rzędu danych w odpowiedzi z bazy to się wykona kod w "else":
                    echo "Wszyscy pacjenci nie mają podwyższonej temperatury. <br><br>";
                }
                mysqli_free_result($wynikZapytania);    //czyszczę dane odpowiedzi z bazy











                //Zapytanie nr 3.:
                //mysqli_query daje zapytanie do bazy o imię i ciśnienie skurczowe oraz rozkurczowe osoby, które nie mieszczą się w prawidłowych przedziałach - odpowiednio 120-129 oraz 80-84
                //Może to służyć do tego żeby personel wiedział, którym osobom zrobić badania np na nadciśnienie tętnicze.
                $wynikZapytania = mysqli_query($polaczenie, 
                "SELECT Imie, cisnienieSkurczowe, cisnienieRozkurczowe FROM $dbtable WHERE !(cisnienieSkurczowe>=120 AND cisnienieSkurczowe<=129) OR !(cisnienieRozkurczowe >= 80 AND cisnienieRozkurczowe <= 84) ORDER BY Imie");

                echo "Osoby mające niepoprawne ciśnienie: <br>";
                if (mysqli_num_rows($wynikZapytania) > 0) {     //Jeśli jest więcej niż 0 rzędów danych w odpowiedzi z bazy to wykonuje się to co w IF'ie

                    
                    echo "<table border='1'>
                            <tr>
                                <td>Imię:</td> <td>Ciśnienie:</td>
                            </tr>";

                    while ($row = mysqli_fetch_assoc($wynikZapytania)) {    //pętla wykonuje się dla każdego rzędu odpowiedzi z bazy. Dostęp do rzędu uzyskujemy dzięki funkcji mysql_fetch_assoc, która zamienia rząd odpowiedzi na array (w tym przypadku "$row"), do którego mamy dostęp
                        
                        $Imie = $row["Imie"];   //przypisuję wartości elementów z array'a do zmiennych
                        $cisS = $row["cisnienieSkurczowe"];
                        $cisR = $row["cisnienieRozkurczowe"];

                        echo "<tr>
                                <td>$Imie</td> <td>$cisS/$cisR</td>
                             </tr>";    //wypisuję dane do tabeli
                    }

                    echo "</table> <br><br>";
                
                }else{ //Jeśli nie będzie ani jednego rzędu danych w odpowiedzi z bazy to się wykona kod w "else":
                    echo "Wszyscy pacjenci mają prawidłowe ciśnienie. <br><br>";
                }
                mysqli_free_result($wynikZapytania);    //czyszczę dane odpowiedzi z bazy












                //Zapytanie nr 4.:
                //mysqli_query daje zapytanie do bazy o poziom stresu oraz średni puls z przedziału od 60 do 100, następnie grupuje wyniki odpowiedzi według poziomu stresu
                //Może to służyć do tego żeby personel widział jak kształtuje się średni puls (w prawidłowym przedziale) u pacjentów względem poziomu stresu.
                $wynikZapytania = mysqli_query($polaczenie, "SELECT poziomStresu, avg(puls) FROM $dbtable WHERE (puls>=60 AND puls<=100) GROUP BY poziomStresu");

                echo "Średni puls w prawidłowym przedziale u pacjentów przy danym poziomie stresu: <br>";
                if (mysqli_num_rows($wynikZapytania) > 0) {     //Jeśli jest więcej niż 0 rzędów danych w odpowiedzi z bazy to wykonuje się to co w IF'ie

                    

                    
                    echo "<table border='1'>
                            <tr style='text-align:center;'>
                                <td>Poziom stresu od 1 do 5:</td> <td>średni puls:</td>
                            </tr>";

                    while ($row = mysqli_fetch_assoc($wynikZapytania)) {    //pętla wykonuje się dla każdego rzędu odpowiedzi z bazy. Dostęp do rzędu uzyskujemy dzięki funkcji mysql_fetch_assoc, która zamienia rząd odpowiedzi na array (w tym przypadku "$row"), do którego mamy dostęp
                        

                        $stres = $row["poziomStresu"];   //przypisuję wartości elementów z array'a do zmiennych
                        $puls = $row["avg(puls)"];

                        echo "<tr style='text-align:center;'>
                                <td>$stres</td> <td>$puls</td>
                             </tr>";    //wypisuję dane do tabeli
                    }

                    echo "</table> <br><br>";
                
                }else{ //jeśli nie będzie pacjenta z prawidłowym pulsem to zostanie wykonane:
                    echo "Pacjenci mają nieprawidłowy puls";
                }
                mysqli_free_result($wynikZapytania);    //czyszczę dane odpowiedzi z bazy













                //Zapytanie nr 5.:
                //mysqli_query daje zapytanie do bazy o średnią wagę, średni wzrost, średnie BMI oraz o maksymalną i minimalną ilość wypitych szklanek wody wśród pacjentów

                //Może to służyć do tego żeby porównywać do siebie średnie wzrostu, wagi, BMI na przestrzeni lat i obserwować tendencje 
                //oraz do tego żeby sprawdzać czy jest potrzeba promowania picia odpowiedniej ilości wody
                $wynikZapytania = mysqli_query($polaczenie, 
                "SELECT avg(waga), sum(wzrost)/count(wzrost) AS srwzrost, avg(waga)/((avg(wzrost)/100)*(avg(wzrost)/100)) AS srBMI, max(iloscSzklanekWody) AS woda, min(iloscSzklanekWody) AS wodaMin FROM $dbtable");

                echo "Średnie parametry u pacjentów: <br>";
                if (mysqli_num_rows($wynikZapytania) > 0) {     //Jeśli jest więcej niż 0 rzędów danych w odpowiedzi z bazy to wykonuje się to co w IF'ie

            
                    echo "<table border='1'>
                            <tr style='text-align:center;'>
                                <td>Średnia waga:</td> <td>Średni wzrost:</td> <td>Średnie BMI:</td> <td>max ilość szklanek wody:</td> <td>min ilość szklanek wody:</td>
                            </tr>";

                    while ($row = mysqli_fetch_assoc($wynikZapytania)) {    //pętla wykonuje się dla każdego rzędu odpowiedzi z bazy. Dostęp do rzędu uzyskujemy dzięki funkcji mysql_fetch_assoc, która zamienia rząd odpowiedzi na array (w tym przypadku "$row"), do którego mamy dostęp
                        

                        $waga = $row["avg(waga)"];   //przypisuję wartości elementów z array'a do zmiennych
                        $wzrost = $row["srwzrost"];
                        $BMI = $row["srBMI"];
                        $maxSZ = $row["woda"];
                        $minSZ = $row["wodaMin"];

                        if($BMI < 18.5){
                            $BMI = $BMI . " - niedowaga";   //sprawdzam ile jest równe BMI i w zależności od tego dopisuję do zmiennej czy jest to niedowaga, w normie, bądź nadwaga
                        }elseif($BMI > 25){
                            $BMI = $BMI . " - nadwaga";
                        }else{
                            $BMI = $BMI . " - w normie";
                        }

                        echo "<tr style='text-align:center;'>
                                <td>$waga</td> <td>$wzrost</td> <td>$BMI</td> <td>$maxSZ</td> <td>$minSZ</td>
                             </tr>";    //wypisuję dane do tabeli
                    }

                    echo "</table> <br><br>";
                
                }
                mysqli_free_result($wynikZapytania);    //czyszczę dane odpowiedzi z bazy









		    }else{  //jeśli połączenie z bazą nie zostanie nawiązane to wykona się kod w else:

			    echo "Blad laczenia do bazy";

		    }

            mysqli_close($polaczenie); //zamykam połączenie z bazą danych

        }else{
    ?>

    <body>

    <main style="width:1500px;margin: 0 auto;">

        <div style="width:500px;height:500px;float:left;background-image:url('medic.png');background-repeat: no-repeat;"></div>
        

        <div style="width:500px;height:500px;float: left;margin-top: 70px;">
        

            <form action="zdrowie.php" method="post" style="text-align: left;width:310px;height:500px;margin: 0 auto;">
        
                Wprowadź datę: <input type="date" name="data" required value="<?php echo date("Y-m-d"); ?>"><br>
                i godzinę: <input type="time" name="czas" required value="<?php echo date("H:i"); ?>"><br><br>
        
                Wprowadź swoje dane:<br><br>

                Imię: <input style="width:100px;" type="text" name="imie" required><br>

                Waga: <input style="width:45px;" type="number" name="waga" required step="any" value="50">kg<br>

                Wzrost: <input style="width:45px;" type="number" name="wzrost" required step="any" value="150">cm<br>

                Temperatura ciała: <input style="width:45px;" type="number" name="temperatura" required step="0.1" value="36.6">°C<br>

                Puls: <input style="width:45px;" type="number" name="puls" required step="any" value="80">BPM<br>

                Ciśnienie tętnicze: 
                <input style="width:45px;" type="number" name="cisnienie1" required step="any" value="120">/<input style="width:45px;" type="number" name="cisnienie2" required step="any" value="80">mmHg<br>

                Saturacja krwi: <input style="width:45px;" type="number" name="saturacja" required step="any" value="100">% <br><br>
        

                <div style="margin-left:102px;">
                1----2----3----4----5
                </div>

                Poziom stresu: <input type="range" min="1" max="5" value="1" step="1" name="stres"><br>
        
                Ilość wypitych dzisiaj szklanek wody: 
                <input style="width:45px;" type="number" name="szklanki" required step="any" value="4">
        
                <br><br>
                <input type="submit" value="prześlij formularz">

            </form>


        </div>


        <div style="width:500px;height:500px;float:left;background-image:url('medic.png');background-repeat: no-repeat;"></div>
    
    </main>

    </body>

    <?php
        }
    ?>

</html>