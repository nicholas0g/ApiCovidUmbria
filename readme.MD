Applicativo per l'invio dei risultati al sistema regione Umbria [qui i dettagli](https://apistore.regione.umbria.it/store/apis/info?name=COVID-19-LIS&version=1.0.0&provider=admin&tag=Salute-group)

:poop: ATTENZIONE: QUESTO SOFTWARE E' UN ESEMPIO SPERIMENTALE, SERVE COME LINEA GUIDA PER IMPLEMENTARE UN SOFTWARE DI PRODUZIONE
## Utilizzo della build:
`ApiCovidUmbria.exe chiaveconsuemer chiavesecret url jsonstring`

L'url non deve contenere caratteri di slah ('/') finali, va impostata in base all'ambiente di produzione o sandbox

## Descrizione funzioni
All'interno del programma principale di progetto sono presenti le funzioni:
1. GeneraToken (serve per generare il token)
2. RisultatoTest (serve a chiamare l'endpoint api POST /risultato-test della regione)
3. EseguiOperazioneAsync (si occupa di sincronizzare l'ottenimento del token e l'invio della post)
4. Base64Encode (serve per fare un encoding della chiave consumer e secret e viene utilizzata nella richiesta del token)


Il programma ApiCovidUmbria.exe genera nella cartella in cui è eseguito un file chiamato token.txt (contiene il token, a livello di test) e un file risposta.txt che contiene invece la risposta delle api regionali. Appena l'esecuzione termina e i file sono generati la finestra del programma si chiude in autonomia, altrimenti mostra un messaggio di errore.
