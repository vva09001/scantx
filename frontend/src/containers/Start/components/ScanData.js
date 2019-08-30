import React from 'react';
import Grid from "@material-ui/core/Grid";
import Papersheet from "components/utility/papersheet";
import { CircularProgress } from 'components/uielements/progress';
import { connect } from 'react-redux';
import { dataActions } from 'redux/actions';
import _ from 'lodash';

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
            loading: true
        }
    }

    componentDidMount() {
        this.props.getData({}, this.onSuccess, this.onSuccess)
    }

    onSuccess = () => {
        this.setState({ loading: false })
    }
    renderData = () => {
        return _.map(this.props.datas, item => {
            return <p>{item.timeScan} {item.code}</p>
        })
    }
    render() {
        return (
            <Grid item xs={6} style={style.gridItem}>
                {
                    this.state.loading ? (
                        <CircularProgress />
                    ) : (
                        <Papersheet title="Scan Input: 123" style={style.papersheet}>
                            {this.renderData()}
                        </Papersheet>
                    )
                }
                
            </Grid>
        )
    }
}

const mapSateToProps = state => {
    return {
        datas: state.Data.datas
    };
};

const mapDispatchToProps = {
    getData : dataActions.getData
};
export default connect(
    mapSateToProps,
    mapDispatchToProps
)(ScanData);