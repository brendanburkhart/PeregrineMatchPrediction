# PeregrineMatchPrediction
Applying ML to data from the Peregrine scouting system to predict the outcome of FRC matches

DataPreparation parses and prepares JSON data from Peregrine for consumption by the ML pipeline.
Training uses the prepared data to train a Support Vector Machine model, which is then saved.
Prediction uses the trained and saved model to make and test the predictions of the model against a different data set.

The raw Peregrine data used is the JSON response from the `/events/{event}/stats` and `/events/{event}/matches` endpoints.

The schema fed in is the primary 2019 schema, however it only contains a few of the fields from the original schema to eliminate data features which by correlated too highly.
```{
  "schema": [
    {
      "name": "Cargo Placed"
    },
    {
      "name": "Hatches Placed"
    },
    {
      "name": "Climbed Lvl 1"
    },
    {
      "name": "Climbed Lvl 2"
    },
    {
      "name": "Climbed Lvl 3"
    },
    {
      "name": "Teleop Hatches"
    },
    {
      "name": "Teleop Cargo"
    }
  ]
}
```

As the data sets available from a single FRC event are quite small, the models tend to overfit somewhat and experience significantly degraded performance on data sets from other events.

Trained on data from Chezy Champs 2019, the results were:
- 2019 PNW DCMP: `F1: 0.6707317073170732 Accuracy: 0.6223776223776224`
- 2019 Peak Performance: `F1: 0.8275862068965518 Accuracy: 0.7959183673469388`
- 2019 Chezy Champs: `F1: 1 Accuracy: 1`

In scenarios I explored, including training the models directly on the PNW DCMP data, the model always performed the worst on the PNW DCMP data. Combining several large events, suchs as all DCMPs, into a single large training data set may provide the best results, however such quantities of data would not be available to Peregrine for the majority of the season.

### Potential improvements
- use historic ranking data from past events to help 'seed' initial results, weight that less and less as more data is collected for that season
- combine and use larger data sets
- auto-select non-correlated features by simply not using computed properties as features
- explore other models - random tree, perceptron
