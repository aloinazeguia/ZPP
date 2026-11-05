2. ariketa - Dadoak
Osaketaren baldintzak
Entregatze-data: igandea, 2026(e)ko azaroaren 8(e)an, 23:59(e)tan
Ataza hau zerbitzariak sortutako ausazko zenbaki bat asmatzean datza, jokaldi horretara konektatutako bezeroen artean asma dezaketena.

ZERBITZARIA
Zerbitzariak mezu bat bidaltzen dio bezeroari, erabiltzaileak 1 eta 100 arteko zenbaki bat igar dezan.
Zerbitzariak hainbat konexio onartuko ditu, eta horietako bakoitzeko hari bat sortuko du.
Berrikusi bezeroak jokoaren hasieran sortutako ausazko zenbakia asmatu duen.
Begiratu jokoa amaitu den. Jokoa amaitu bada, jakinarazi bezeroei jokoa amaitu dela eta ausaz asmatu dutela sortutako zenbakia zerbitzaritik random baten bidez.
Programan akatsen bat gertatuz gero, bezeroari jakinaraziko zaio.
Programa jokalari batek zenbakia asmatzean amaituko da.
EXTRA: egin ranking bat, gertuen geratu diren pertsonena. Ranking horren arabera puntuazio bat emango zaie eta partida arteko rankinga era garatu dezakezue.

BEZEROA
Egiaztatu jokoa amaitu ez den edo errore bat gertatu den.
Zerbitzariaren erantzuna eta lagina jasotzen ditu.
Asmatu badu, erakutsiko du: Irabazi duzu!! Jokoa amaitu da.
Beste jokalari batek irabazi badu, hau erakutsiko du: Jokoa amaitu da, zenbakia asmatu dute. Eta irabazi duen bezeroaren identifikatzailea.
Bezero bakoitzak identifikatzaile bat izango du, bezero bakoitzaren socketarekin lotuta egon daiteke edo ausazko zenbaki bat izan daiteke.
Partida amaitzean, bezeroak zenbat saiakera egin dituen erakutsiko du.
Ataza honetan bi proiektu dituen irtenbide bakarra egin behar duzu. Lehenengo proiektua bezeroa izango da eta bigarrena zerbitzaria. Bi proiektuek TCP socket-ak erabiliz komunikatu behar dute. Socket sinkronoak edo asinkronoak erabil daitezke, baina zerbitzariak gehienez 10 bezerori zerbitzua emateko gai izan behar du. Bezeroek aldi berean lan egin ahal izango dute, eta kontuz ibili behar duzu blokeorik egon ez dadin.
 
 
Sortu beharreko Klaseak:
Zerbitzaria: 10 bezerori konektatzea ahalbidetuko die (aurreko ariketan bezela baina orain hariak erabiliz).
Datuak metodoa izango du, bertan jokoaren  logika nagusia izango da.
Partida: partidaren informazioa osoa+metodo nagusiak (partaideak, zenbakia, irabazlea...) kudeatuko ditu.
--------------------------------------------------------------------------------------

Bezeroa: zerbitzarira konektatuko da. 
Erabiltzarilearen zenbakiak jaso eta zerbitzarira bidaltzeaz arduratuko da, eta zenbakia handiagoa edo txikiagoa den esango dio asmatu arte.
BezroPartida: bezeroaren jokaldi kopurua, partidaren partaideak... biltegiratzen ditu.
Egin beharreko entregak:

Sortu bezero eta zerbitzaria aldibereko 10 konexio onar ditzan
Sortu metdo bat bezero eta zerbitzarian, bezeroek zenbai bat bidaltzeko eta zerbitzariak zenbaki hori inprimatzeko.
Sortu 2 objetu (klase):
Bat bezeroaren informazioa gordeko duena
Beste bat partidaren informazioa gordetzeko