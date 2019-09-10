import React from "react";
import LayoutWrapper from "components/utility/layoutWrapper";
import Grid from "@material-ui/core/Grid";
import { connect } from 'react-redux';
import { UserProfile, ScanData, Qr } from './components';

const style = {
  gridItem: {
    padding: 10,
  },
  papersheet: {
    height: '100%'
  },
};

class Start extends React.Component {
  render(){
    return (
      <LayoutWrapper>
        <Grid container>
          <UserProfile />
          <Qr />
          <ScanData />
        </Grid>
      </LayoutWrapper>
    );
  }
};

const mapStateToProps = state => {
  return {
  };
};

const mapDispatchToProps = {
};
export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Start);