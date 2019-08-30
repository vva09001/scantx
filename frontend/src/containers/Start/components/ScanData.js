import React from 'react';
import Grid from "@material-ui/core/Grid";
import Papersheet from "components/utility/papersheet";
import { CircularProgress } from 'components/uielements/progress';
import { connect } from 'react-redux';

const style = {
    gridItem: {
      padding: 10,
    },
    papersheet: {
      height: '100%'
    },
  };

class ScanData extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: false
        }
    }

    componentDidMount() {
    }

    onSuccess = () => {
        this.setState({ loading: false })
    }
    render() {
        if (this.state.loading) {
            return <CircularProgress />;
        }
        return (
            <Grid item xs={6} style={style.gridItem}>
                <Papersheet title="Scan Input: 123" style={style.papersheet}>
                <p>Lorem Ipsum...</p>
                <p>Lorem Ipsum...</p>
                <p>Lorem Ipsum...</p>
                <p>...</p>
                </Papersheet>
            </Grid>
        )
    }
}

const mapSateToProps = state => {
    return {
    };
};

const mapDispatchToProps = {
};
export default connect(
    mapSateToProps,
    mapDispatchToProps
)(ScanData);