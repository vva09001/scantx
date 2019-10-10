import React from "react";
import LayoutWrapper from "components/utility/layoutWrapper";
import Grid from "@material-ui/core/Grid";
import { connect } from "react-redux";
import { UserProfile, ScanData, Qr } from "./components";

class Start extends React.Component {
  render() {
    return (
      <LayoutWrapper>
        <h1>demo auto deploy</h1>
        <Grid container>
          <UserProfile />
          <Qr />
          <ScanData />
        </Grid>
      </LayoutWrapper>
    );
  }
}

const mapStateToProps = state => {
  return {};
};

const mapDispatchToProps = {};
export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Start);
