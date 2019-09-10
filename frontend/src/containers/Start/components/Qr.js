import React from "react";
import Grid from "@material-ui/core/Grid";
import Papersheet from "components/utility/papersheet";
import { connect } from "react-redux";
import { scanDataActions } from "redux/actions";
import _ from "lodash";

const style = {
  gridItem: {
    padding: 10
  },
  papersheet: {
    height: "100%"
  }
};

class Qr extends React.Component {
  constructor(props) {
    super(props);
  }

  componentDidMount() {
    this.props.getQr(this.onSuccess, this.onSuccess);
  }

  onSuccess = () => {
    this.setState({ loading: false });
  };
  render() {
    return (
      <Grid item xs={6} style={style.gridItem}>
        <Papersheet title="Connect QR" style={style.papersheet}>
          <Grid container direction="row">
            <Grid item xs={6}>
              <p>
                Scan this QR code to connect your ScanTX app with your computer
              </p>
            </Grid>
            <Grid item xs={6}>
              <img
                width="100%"
                src={"data:image/png;base64, " + this.props.qr}
              ></img>
            </Grid>
          </Grid>
        </Papersheet>
      </Grid>
    );
  }
}

const mapStateToProps = state => {
  return {
    qr: state.ScanData.qr
  };
};

const mapDispatchToProps = {
  getQr: scanDataActions.getQr
};
export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Qr);
