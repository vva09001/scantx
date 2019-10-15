import React from "react";
import Grid from "@material-ui/core/Grid";
import Papersheet from "components/utility/papersheet";
import { CircularProgress } from "components/uielements/progress";
import { connect } from "react-redux";
import { scanDataActions } from "redux/actions";
import { Date, Time } from "helpers/moment";
import _ from "lodash";

const style = {
  gridItem: {
    padding: 10
  },
  papersheet: {
    height: "100%"
  }
};

class ScanData extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      loading: true
    };
  }

  componentDidMount() {
    this.props.getScanData(this.onSuccess, this.onSuccess);
    // try {
    //   setInterval(async () => {
    //     await this.props.getScanData(this.onSuccess, this.onSuccess);
    //   }, 10000);
    // } catch (error) {}
  }

  onSuccess = () => {
    this.setState({ loading: false });
  };
  renderData = () => {
    return _.map(this.props.datas, item => {
      return (
        <p>
          {Date(item.createdOn)} {Time(item.createdOn)} {item.payload}
        </p>
      );
    });
  };
  render() {
    return (
      <Grid item xs={6} style={style.gridItem}>
        {this.state.loading ? (
          <CircularProgress />
        ) : (
          <Papersheet title="Scan Input" style={style.papersheet}>
            {this.renderData()}
          </Papersheet>
        )}
      </Grid>
    );
  }
}

const mapSateToProps = state => {
  return {
    datas: state.ScanData.list
  };
};

const mapDispatchToProps = {
  getScanData: scanDataActions.getScanData
};
export default connect(
  mapSateToProps,
  mapDispatchToProps
)(ScanData);
